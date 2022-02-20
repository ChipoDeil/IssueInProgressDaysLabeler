using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Execution;
using IssueInProgressDaysLabeler.Model.Extensions;
using IssueInProgressDaysLabeler.Model.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IssueInProgressDaysLabeler.Model
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var configuration = await FetchSettings(args);

            if (configuration == null)
                return StatusCodeConstants.ErrorBadArgumentsStatusCode;

            await Run(configuration);

            return StatusCodeConstants.SuccessStatusCode;
        }

        private static async Task Run(IssueInProgressConsoleSettings configuration)
        {
            await using var services = new ServiceCollection()
                .ConfigureApp(configuration)
                .BuildServiceProvider();

            var executor = services.GetRequiredService<IAppExecutor>();

            await executor.Execute();
        }

        private static async Task<IssueInProgressConsoleSettings?> FetchSettings(string[] args)
        {
            await using var rootServices = new ServiceCollection()
                .AddSingleton<ISettingsParser, SettingsParser>()
                .AddLogging(c => c.AddConsole())
                .BuildServiceProvider();

            var configuration = rootServices.GetRequiredService<ISettingsParser>().TryParse(args);

            return configuration;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using Newtonsoft.Json;

namespace IssueInProgressDaysLabeler.Model
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(args));

            using var parser = new Parser(with =>
            {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out;
            });

            var result = parser.ParseArguments<Options>(args)
                    .MapResult(OnParsed, OnError);

            return result;
        }

        private static async Task<int> OnParsed(Options options)
        {
            var settings = ParseSettings(options);

            Console.WriteLine($"Labels: {JsonConvert.SerializeObject(settings.Labels)}");

            var gitHubClientFacade = GithubClientFactory.Create(
                settings.GithubToken,
                settings.Owner,
                repositoryName: settings.Repository);

            var daysModeHelper = new DaysModeHelper(settings.DaysMode);

            await IncrementDaysStrategy.IncrementDays(
                gitHubClientFacade,
                daysModeHelper,
                settings.Labels);

            return StatusCodeConstants.SuccessStatusCode;
        }

        private static Task<int> OnError(IEnumerable<Error> errors)
            => Task.FromResult(StatusCodeConstants.ErrorBadArgumentsStatusCode);

        private static Settings ParseSettings(Options options)
        {
            return new(
                options.Owner,
                options.Repository.Replace($"{options.Owner}/", string.Empty),
                labels: JsonConvert.DeserializeObject<string[]>(options.Labels),
                options.GithubToken,
                options.DaysMode);
        }
    }
}
using IssueInProgressDaysLabeler.Model.Execution;
using IssueInProgressDaysLabeler.Model.Github;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies.DaysProcessing;
using IssueInProgressDaysLabeler.Model.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureApp(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            serviceCollection.AddLogging(c => c.AddConsole());
            serviceCollection.AddGithubClientFacade(settings);
            serviceCollection.AddUpdateStrategies(settings);
            serviceCollection.AddExecutor(settings);

            return serviceCollection;
        }

        private static void AddGithubClientFacade(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            var credentials = new Credentials(settings.GithubToken);
            var productHeaderValue = new ProductHeaderValue(GithubConstants.AppName);
            var githubClient = new GitHubClient(productHeaderValue)
            {
                Credentials = credentials
            };

            serviceCollection.AddSingleton(githubClient);
            serviceCollection.AddSingleton(_ => new GithubClientAdapterSettings(
                settings.Owner,
                settings.Repository));

            serviceCollection.AddSingleton<IGithubClientAdapter, GithubClientAdapter>();
        }

        private static void AddUpdateStrategies(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            serviceCollection.AddSingleton(_ => new DaysModeHelperSettings(settings.DaysMode));
            serviceCollection.AddSingleton<IDaysModeHelper, DaysModeHelper>();

            serviceCollection.AddSingleton(_ => new IncrementDaysStrategySettings(settings.LabelToIncrement));

            serviceCollection.AddSingleton<IssueUpdateStrategy, IncrementDaysStrategy>();

            if (!settings.AutoCleanup) return;

            serviceCollection.AddSingleton(_ => new CleanUpLabelStrategySettings(settings.LabelToIncrement));
            serviceCollection.AddSingleton<IssueUpdateStrategy, CleanUpLabelStrategy>();
        }

        private static void AddExecutor(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            serviceCollection.AddSingleton(_ => new AppExecutorSettings(settings.Labels, settings.Since));
            serviceCollection.AddSingleton<IAppExecutor, AppExecutor>();
        }
    }
}
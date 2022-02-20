using System.Linq;
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
    // TODO: do not create objects: use options (?)
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

            var facade = new GithubClientFacade(githubClient, settings.Owner, settings.Repository);

            serviceCollection.AddSingleton<IGithubClientFacade>(facade);
        }

        private static void AddUpdateStrategies(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            serviceCollection.AddSingleton<IDaysModeHelper>(_ =>
                new DaysModeHelper(settings.DaysMode));

            serviceCollection.AddSingleton<IssueUpdateStrategy>(c =>
                new IncrementDaysStrategy(
                    logger: c.GetRequiredService<ILogger<IncrementDaysStrategy>>(),
                    daysModeHelper: c.GetRequiredService<IDaysModeHelper>(),
                    settings.LabelToIncrement));

            if (settings.AutoCleanup)
                serviceCollection.AddSingleton<IssueUpdateStrategy>(c =>
                    new CleanUpLabelStrategy(
                        logger: c.GetRequiredService<ILogger<CleanUpLabelStrategy>>(),
                        labelTemplate: settings.LabelToIncrement));
        }

        private static void AddExecutor(
            this IServiceCollection serviceCollection,
            IssueInProgressConsoleSettings settings)
        {
            serviceCollection.AddSingleton<IAppExecutor>(c =>
                new AppExecutor(
                    updateStrategies: c.GetServices<IssueUpdateStrategy>().ToArray(),
                    githubClientFacade: c.GetRequiredService<IGithubClientFacade>(),
                    settings.Labels,
                    settings.Since));
        }
    }
}
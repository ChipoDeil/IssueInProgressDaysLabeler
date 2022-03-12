using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Github;
using IssueInProgressDaysLabeler.Model.IssueUpdatePreprocessors;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies;

namespace IssueInProgressDaysLabeler.Model.Execution
{
    internal class AppExecutor : IAppExecutor
    {
        private readonly IEnumerable<IIssueUpdatePreprocessor> _preprocessors;
        private readonly IEnumerable<IssueUpdateStrategy> _updateStrategies;
        private readonly IGithubClientAdapter _githubClientAdapter;
        private readonly IReadOnlyCollection<string> _labels;
        private readonly DateTimeOffset? _since;

        public AppExecutor(
            IEnumerable<IIssueUpdatePreprocessor> preprocessors,
            IEnumerable<IssueUpdateStrategy> updateStrategies,
            IGithubClientAdapter githubClientAdapter,
            AppExecutorSettings settings)
        {
            _preprocessors = preprocessors ?? throw new ArgumentNullException(nameof(preprocessors));
            _updateStrategies = updateStrategies ?? throw new ArgumentNullException(nameof(updateStrategies));
            _githubClientAdapter = githubClientAdapter ?? throw new ArgumentNullException(nameof(githubClientAdapter));
            (_labels, _since) = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task Execute()
        {
            var issuesToUpdate = await _githubClientAdapter
                .GetIssuesToUpdate(_labels, _since);

            foreach (var issue in issuesToUpdate)
            {
                foreach (var preprocessor in _preprocessors)
                {
                    preprocessor.Preprocess(issue);
                }
            }

            foreach (var issue in issuesToUpdate)
            {
                foreach (var strategy in _updateStrategies)
                {
                    strategy.TryUpdateIssue(issue);
                }
            }

            await Task.WhenAll(_githubClientAdapter.UpdateIssues(issuesToUpdate));
        }
    }
}
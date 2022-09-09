using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Dtos;
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

            var actuallyUpdatedIssues = new List<IssueUpdateWithNumber>(capacity: 100);

            foreach (var issue in issuesToUpdate)
            {
                var actuallyUpdated = _updateStrategies.Select(c => c.TryUpdateIssue(issue)).Any(c => c);

                if (actuallyUpdated)
                {
                    actuallyUpdatedIssues.Add(issue);
                }
            }

            await _githubClientAdapter.UpdateIssues(actuallyUpdatedIssues);
        }
    }
}
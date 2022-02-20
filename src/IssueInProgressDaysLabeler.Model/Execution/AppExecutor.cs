using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Github;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies;

namespace IssueInProgressDaysLabeler.Model.Execution
{
    internal class AppExecutor : IAppExecutor
    {
        private readonly IReadOnlyCollection<IssueUpdateStrategy> _updateStrategies;
        private readonly IGithubClientFacade _githubClientFacade;
        private readonly IReadOnlyCollection<string> _labels;
        private readonly DateTimeOffset? _since;

        public AppExecutor(
            IReadOnlyCollection<IssueUpdateStrategy> updateStrategies,
            IGithubClientFacade githubClientFacade,
            IReadOnlyCollection<string> labels,
            DateTimeOffset? since)
        {
            _updateStrategies = updateStrategies;
            _githubClientFacade = githubClientFacade;
            _labels = labels;
            _since = since;
        }

        public async Task Execute()
        {
            var issuesToUpdate = await _githubClientFacade
                .GetIssuesToUpdate(_labels, _since);

            foreach (var issue in issuesToUpdate)
            {
                foreach (var strategy in _updateStrategies)
                {
                    strategy.TryUpdateIssue(issue);
                }
            }

            await Task.WhenAll(_githubClientFacade.UpdateIssues(issuesToUpdate));
        }
    }
}
using System;
using IssueInProgressDaysLabeler.Model.Dtos;
using Microsoft.Extensions.Logging;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies
{
    internal class CleanUpLabelStrategy : IssueUpdateStrategy
    {
        private readonly ILogger<CleanUpLabelStrategy> _logger;
        private readonly string _labelTemplate;

        public CleanUpLabelStrategy(
            ILogger<CleanUpLabelStrategy> logger,
            CleanUpLabelStrategySettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _labelTemplate = settings.LabelTemplate;
        }

        public override void TryUpdateIssue(IssueUpdateWithNumber issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            if (issue.IssueState == IssueState.Opened)
                return;

            var labelToCleanUp = TryGetLabelByTemplate(issue, _labelTemplate);
            if (labelToCleanUp == null)
                return;

            issue.IssueUpdate.Labels.Remove(labelToCleanUp);
            _logger.LogInformation($"Issue #{issue.Number}: removed days label");
        }
    }
}
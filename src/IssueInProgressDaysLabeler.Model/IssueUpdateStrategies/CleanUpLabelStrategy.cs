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

        public override bool TryUpdateIssue(IssueUpdateWithNumber issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            if (issue.IssueState == IssueState.Opened)
            {
                _logger.LogInformation($"Issue #{issue.Number} skipped: opened");
                return false;
            }

            var labelToCleanUp = TryGetLabelByTemplate(issue, _labelTemplate);
            if (labelToCleanUp == null)
            {
                _logger.LogInformation($"Issue #{issue.Number} skipped: no label");
                return false;
            }

            issue.IssueUpdate.Labels.Remove(labelToCleanUp);
            _logger.LogInformation($"Issue #{issue.Number}: removed days label");

            return true;
        }
    }
}
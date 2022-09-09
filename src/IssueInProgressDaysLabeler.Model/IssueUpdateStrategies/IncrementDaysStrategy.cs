using System;
using System.Linq;
using System.Text.RegularExpressions;
using IssueInProgressDaysLabeler.Model.Dtos;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies.DaysProcessing;
using Microsoft.Extensions.Logging;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies
{
    internal class IncrementDaysStrategy : IssueUpdateStrategy
    {
        private readonly ILogger<IncrementDaysStrategy> _logger;
        private readonly IDaysModeHelper _daysModeHelper;
        private readonly string _labelTemplate;

        public IncrementDaysStrategy(
            ILogger<IncrementDaysStrategy> logger,
            IDaysModeHelper daysModeHelper,
            IncrementDaysStrategySettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _daysModeHelper = daysModeHelper ?? throw new ArgumentNullException(nameof(daysModeHelper));
            _labelTemplate = settings.LabelTemplate;
        }

        public override bool TryUpdateIssue(IssueUpdateWithNumber issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            if (issue.IssueState != IssueState.Opened
                || !_daysModeHelper.IsSuitableDay(DateTime.UtcNow))
            {
                _logger.LogInformation($"Issue #{issue.Number} skipped");
                return false;
            }

            var daysCount = 1;
            var labels = issue.IssueUpdate.Labels;
            var outdatedLabel = TryGetLabelByTemplate(issue, _labelTemplate);
            if (outdatedLabel != null)
            {
                labels.Remove(outdatedLabel);
                var findDigitRegex = new Regex(DigitFormat);
                var daysFromLabel = int.Parse(findDigitRegex.Match(outdatedLabel).Groups.Values.Single().Value);
                daysCount += daysFromLabel;
            }

            labels.Add(string.Format(_labelTemplate, daysCount));

            _logger.LogInformation($"Issue #{issue.Number}: set days label to {daysCount}");

            return true;
        }
    }
}
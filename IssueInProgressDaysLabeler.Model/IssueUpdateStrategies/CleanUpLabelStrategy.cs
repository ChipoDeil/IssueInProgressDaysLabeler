using System;
using Octokit;

namespace IssueInProgressDaysLabeler.Model
{
    internal class CleanUpLabelStrategy : IssueUpdateStrategy
    {
        private readonly string _labelTemplate;

        public CleanUpLabelStrategy(string labelTemplate)
        {
            if (string.IsNullOrWhiteSpace(labelTemplate))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(labelTemplate));
            _labelTemplate = labelTemplate;
        }

        public override void TryUpdateIssue(IssueUpdateWithNumber issue)
        {
            if (issue == null)
                throw new ArgumentNullException(nameof(issue));

            if (issue.IssueUpdate.State == ItemState.Open)
                return;

            var labelToCleanUp = TryGetLabelByTemplate(issue, _labelTemplate);
            if (labelToCleanUp == null)
                return;

            issue.IssueUpdate.Labels.Remove(labelToCleanUp);
            Console.WriteLine($"Issue #{issue.Number}: removed days label");
        }
    }
}
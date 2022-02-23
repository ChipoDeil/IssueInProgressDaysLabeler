using System;
using System.Linq;
using System.Text.RegularExpressions;
using IssueInProgressDaysLabeler.Model.Dtos;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies
{
    // TODO: composition instead of inheritance
    internal abstract class IssueUpdateStrategy
    {
        protected const string DigitFormat = "\\d+";
        public abstract void TryUpdateIssue(IssueUpdateWithNumber issue);

        protected static string? TryGetLabelByTemplate(IssueUpdateWithNumber issue, string labelTemplate)
        {
            if (issue == null) throw new ArgumentNullException(nameof(issue));
            if (labelTemplate == null) throw new ArgumentNullException(nameof(labelTemplate));

            var findLabelRegex = new Regex(string.Format(labelTemplate, DigitFormat));
            return issue.IssueUpdate.Labels.FirstOrDefault(findLabelRegex.IsMatch);
        }
    }
}
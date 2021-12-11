using System.Linq;
using System.Text.RegularExpressions;

namespace IssueInProgressDaysLabeler.Model
{
    internal abstract class IssueUpdateStrategy
    {
        protected const string DigitFormat = "\\d+";
        public abstract void TryUpdateIssue(IssueUpdateWithNumber issue);

        protected static string TryGetLabelByTemplate(IssueUpdateWithNumber issue, string labelTemplate)
        {
            var findLabelRegex = new Regex(string.Format(labelTemplate, DigitFormat));
            return issue.IssueUpdate.Labels.FirstOrDefault(findLabelRegex.IsMatch);
        }
    }
}
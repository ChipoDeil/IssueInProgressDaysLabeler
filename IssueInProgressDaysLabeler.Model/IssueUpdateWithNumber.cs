using Octokit;

namespace IssueInProgressDaysLabeler.Model
{
    internal class IssueUpdateWithNumber
    {
        internal IssueUpdateWithNumber(int number, IssueUpdate issueUpdate)
        {
            Number = number;
            IssueUpdate = issueUpdate;
        }

        internal int Number { get; }
        internal IssueUpdate IssueUpdate { get; }
    }
}
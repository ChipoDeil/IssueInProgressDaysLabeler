using Octokit;

namespace IssueInProgressDaysLabeler.Model
{
    internal class IssueUpdateWithId
    {
        internal IssueUpdateWithId(int id, IssueUpdate issueUpdate)
        {
            Id = id;
            IssueUpdate = issueUpdate;
        }

        internal int Id { get; }
        internal IssueUpdate IssueUpdate { get; }
    }
}
using IssueInProgressDaysLabeler.Model.Dtos;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.IssueUpdatePreprocessors
{
    internal class IssueUpdateNullateStatePreprocessor : IIssueUpdatePreprocessor
    {
        // prevent octokit from updating states in closed issues: cases validation error
        public void Preprocess(IssueUpdateWithNumber issueUpdateWithNumber)
        {
            var (_, _, issueUpdate) = issueUpdateWithNumber;

            if (issueUpdate.State == ItemState.Closed)
            {
                issueUpdate.State = null;
            }
        }
    }
}
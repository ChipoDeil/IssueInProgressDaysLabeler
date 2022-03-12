using System;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Dtos
{
    internal record IssueUpdateWithNumber(int Number, IssueState IssueState, IssueUpdate IssueUpdate)
    {
        public static IssueUpdateWithNumber Convert(int number, IssueUpdate issueUpdate)
        {
            var state = issueUpdate.State switch
            {
                ItemState.Closed => IssueState.Closed,
                ItemState.Open => IssueState.Opened,
                _ => throw new ArgumentException(nameof(issueUpdate))
            };

            return new IssueUpdateWithNumber(number, state, issueUpdate);
        }
    }
}
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Dtos
{
    internal record IssueUpdateWithNumber(int Number, IssueUpdate IssueUpdate);
}
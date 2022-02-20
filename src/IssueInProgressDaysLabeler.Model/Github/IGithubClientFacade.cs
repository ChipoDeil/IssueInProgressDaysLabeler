using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Dtos;

namespace IssueInProgressDaysLabeler.Model.Github
{
    internal interface IGithubClientFacade
    {
        Task<IReadOnlyCollection<IssueUpdateWithNumber>> GetIssuesToUpdate(
            IReadOnlyCollection<string> labels,
            DateTimeOffset? since);

        IReadOnlyCollection<Task> UpdateIssues(IReadOnlyCollection<IssueUpdateWithNumber> issuesToUpdate);
    }
}
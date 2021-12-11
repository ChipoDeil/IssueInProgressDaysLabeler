using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Extensions;
using Octokit;

namespace IssueInProgressDaysLabeler.Model
{
    internal sealed class GithubClientFacade
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _repositoryOwner;
        private readonly string _repositoryName;

        internal GithubClientFacade(GitHubClient gitHubClient, string repositoryOwner, string repositoryName)
        {
            _gitHubClient = gitHubClient;
            _repositoryOwner = repositoryOwner;
            _repositoryName = repositoryName;
        }

        internal async Task<IReadOnlyCollection<IssueUpdateWithNumber>> GetIssuesToUpdate(
            IReadOnlyCollection<string> labels,
            DateTimeOffset? since)
        {
            var allIssues = new List<Issue>();

            foreach (var label in labels)
            {
                var issueRequest = new RepositoryIssueRequest
                {
                    Assignee = "*",
                    Since = since
                };

                issueRequest.Labels.Add(label);

                var issues = await ApiHelpers.ExecuteWithRetry(() => _gitHubClient
                    .Issue
                    .GetAllForRepository(
                        _repositoryOwner,
                        _repositoryName,
                        issueRequest,
                        new ApiOptions
                        {
                            PageCount = 2,
                            PageSize = 100,
                            StartPage = 1
                        }),
                    retryCount: 3);

                allIssues.AddRange(issues);
            }

            return allIssues
                .Where(c => c.PullRequest == null)
                .Select(c => new IssueUpdateWithNumber(c.Number, c.ToUpdate())).ToArray();
        }

        internal IReadOnlyCollection<Task> UpdateIssues(IReadOnlyCollection<IssueUpdateWithNumber> issuesToUpdate)
        {
            return issuesToUpdate.Select(UpdateIssue).ToArray();

            Task UpdateIssue(IssueUpdateWithNumber issueUpdateWithNumber)
            {
                return ApiHelpers.ExecuteWithRetry(() => _gitHubClient.Issue.Update(
                    _repositoryOwner,
                    _repositoryName,
                    issueUpdateWithNumber.Number,
                    issueUpdateWithNumber.IssueUpdate), retryCount: 3);
            }
        }
    }
}
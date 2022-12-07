using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IssueInProgressDaysLabeler.Model.Dtos;
using IssueInProgressDaysLabeler.Model.Extensions;
using Octokit;

namespace IssueInProgressDaysLabeler.Model.Github
{
    internal sealed class GithubClientAdapter : IGithubClientAdapter
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _repositoryOwner;
        private readonly string _repositoryName;

        public GithubClientAdapter(
            GitHubClient gitHubClient,
            GithubClientAdapterSettings settings)
        {
            _gitHubClient = gitHubClient ?? throw new ArgumentNullException(nameof(gitHubClient));
            (_repositoryOwner, _repositoryName) = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IReadOnlyCollection<IssueUpdateWithNumber>> GetIssuesToUpdate(
            IReadOnlyCollection<string> labels,
            DateTimeOffset? since)
        {
            var allIssues = new List<Issue>(capacity: 100);

            var issueRequest = new RepositoryIssueRequest
            {
                Since = since,
                State = ItemStateFilter.All
            };

            foreach (var label in labels) 
                issueRequest.Labels.Add(label);

            var issues = await ApiHelpers.ExecuteWithRetryAndDelay(() => _gitHubClient
                    .Issue
                    .GetAllForRepository(
                        _repositoryOwner,
                        _repositoryName,
                        issueRequest),
                retryCount: 3);

            allIssues.AddRange(issues);

            return allIssues
                // TODO: make api call to filter assignees (now github api is not ready)
                .Where(c => c.Assignee != null || c.Assignees.Any())
                .Where(c => c.PullRequest == null)
                .Select(c => IssueUpdateWithNumber.Convert(c.Number, c.ToUpdate())).ToArray();
        }

        public async Task UpdateIssues(
            IReadOnlyCollection<IssueUpdateWithNumber> issuesToUpdate)
        {
            foreach (var item in issuesToUpdate)
            {
                await ApiHelpers.ExecuteWithRetryAndDelay(() => _gitHubClient.Issue.Update(
                    _repositoryOwner,
                    _repositoryName,
                    item.Number,
                    item.IssueUpdate), retryCount: 3);
            }
        }
    }
}
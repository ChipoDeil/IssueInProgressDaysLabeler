using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            IReadOnlyCollection<string> labels)
        {
            var allIssues = new List<Issue>();

            foreach (var label in labels)
            {
                var issueRequest = new RepositoryIssueRequest
                {
                    // octokit does not allow filtering issues by criteria "assigned to anyone"
                    Filter = IssueFilter.All,
                    State = ItemStateFilter.Open
                };

                issueRequest.Labels.Add(label);

                var issues = await _gitHubClient
                    .Issue
                    .GetAllForRepository(
                        _repositoryOwner,
                        _repositoryName,
                        issueRequest);

                allIssues.AddRange(issues);
            }

            return allIssues
                /*
                 * Note: GitHub's REST API v3 considers every pull request an issue,
                 * but not every issue is a pull request. For this reason, "Issues"
                 * endpoints may return both issues and pull requests in the response.
                 * You can identify pull requests by the pull_request key.
                 */
                .Where(c => c.PullRequest == null)
                .Where(c => c.Assignees.Any())
                .Select(c => new IssueUpdateWithNumber(c.Number, c.ToUpdate())).ToArray();
        }

        internal IReadOnlyCollection<Task> UpdateIssues(IReadOnlyCollection<IssueUpdateWithNumber> issuesToUpdate)
        {
            return issuesToUpdate.Select(UpdateIssue).ToArray();

            Task UpdateIssue(IssueUpdateWithNumber issueUpdateWithId)
            {
                return _gitHubClient.Issue.Update(
                    _repositoryOwner,
                    _repositoryName,
                    issueUpdateWithId.Number,
                    issueUpdateWithId.IssueUpdate);
            }
        }
    }
}
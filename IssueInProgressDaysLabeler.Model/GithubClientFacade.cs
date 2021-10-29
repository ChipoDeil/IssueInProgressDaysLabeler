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

        internal async Task<IReadOnlyCollection<IssueUpdateWithId>> GetIssuesToUpdate(
            IReadOnlyCollection<string> labels)
        {
            var allIssues = new List<Issue>();

            foreach (var label in labels)
            {
                var issueRequest = new RepositoryIssueRequest
                {
                    Filter = IssueFilter.Assigned,
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
                .Select(c => new IssueUpdateWithId(c.Id, c.ToUpdate())).ToArray();
        }

        internal IReadOnlyCollection<Task> UpdateIssues(IReadOnlyCollection<IssueUpdateWithId> issuesToUpdate)
        {
            return issuesToUpdate.Select(UpdateIssue).ToArray();

            Task UpdateIssue(IssueUpdateWithId issueUpdateWithId)
            {
                return _gitHubClient.Issue.Update(
                    _repositoryOwner,
                    _repositoryName,
                    issueUpdateWithId.Id,
                    issueUpdateWithId.IssueUpdate);
            }
        }
    }
}
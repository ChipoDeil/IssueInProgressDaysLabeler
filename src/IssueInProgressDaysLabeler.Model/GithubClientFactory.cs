using System;
using Octokit;

namespace IssueInProgressDaysLabeler.Model
{
    internal static class GithubClientFactory
    {
        internal static GithubClientFacade Create(
            string githubToken,
            string repositoryOwner,
            string repositoryName)
        {
            if(githubToken == null)
                throw new ArgumentNullException(nameof(githubToken));

            var credentials = new Credentials(githubToken);
            var productHeaderValue = new ProductHeaderValue(GithubConstants.AppName);
            var githubClient = new GitHubClient(productHeaderValue)
            {
                Credentials = credentials
            };

            return new GithubClientFacade(githubClient, repositoryOwner, repositoryName);
        }
    }
}
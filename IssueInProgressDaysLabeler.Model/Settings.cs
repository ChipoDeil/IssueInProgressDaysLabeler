using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model
{
    internal class Settings
    {
        internal Settings(
            string owner,
            string repository,
            IReadOnlyCollection<string> labels,
            string githubToken)
        {
            Owner = owner;
            Repository = repository;
            Labels = labels;
            GithubToken = githubToken;
        }

        internal string Owner { get; }
        internal string Repository { get; set; }
        internal IReadOnlyCollection<string> Labels { get; set; }
        internal string GithubToken { get; set; }
    }
}
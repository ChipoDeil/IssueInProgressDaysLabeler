using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model
{
    internal class Settings
    {
        internal Settings(
            string owner,
            string repository,
            IReadOnlyCollection<string> labels,
            string githubToken,
            DaysMode daysMode, string labelToIncrement)
        {
            Owner = owner;
            Repository = repository;
            Labels = labels;
            GithubToken = githubToken;
            DaysMode = daysMode;
            LabelToIncrement = labelToIncrement;
        }

        internal string Owner { get; }
        internal string Repository { get; set; }
        internal IReadOnlyCollection<string> Labels { get; set; }
        internal string GithubToken { get; set; }
        internal DaysMode DaysMode { get; set; }
        internal string LabelToIncrement { get; set; }
    }
}
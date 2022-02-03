using System;
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
            DaysMode daysMode,
            string labelToIncrement,
            DateTimeOffset since)
        {
            Owner = owner;
            Repository = repository;
            Labels = labels;
            GithubToken = githubToken;
            DaysMode = daysMode;
            LabelToIncrement = labelToIncrement;
            Since = since;
        }

        internal string Owner { get; }
        internal string Repository { get; }
        internal IReadOnlyCollection<string> Labels { get; }
        internal string GithubToken { get; }
        internal DaysMode DaysMode { get; }
        internal string LabelToIncrement { get; }
        internal DateTimeOffset Since { get; }
    }
}
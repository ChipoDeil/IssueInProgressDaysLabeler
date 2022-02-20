using System;
using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model.Settings
{
    internal record IssueInProgressConsoleSettings(
        string Owner,
        string Repository,
        IReadOnlyCollection<string> Labels,
        string GithubToken,
        DaysMode DaysMode,
        string LabelToIncrement,
        DateTimeOffset? Since,
        bool AutoCleanup);
}
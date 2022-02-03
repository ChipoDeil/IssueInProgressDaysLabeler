using System;
using CommandLine;

namespace IssueInProgressDaysLabeler.Model
{
    public class Options
    {
        [Option("github-repository", Required = true, HelpText = "Repository to increment days in and owner")]
        public virtual string GithubRepositoryName { get; set; }

        [Option("days-mode", Required = true, HelpText = "Enum: EveryDay, EveryDayExceptWeekend, RussianCalendar")]
        public virtual DaysMode DaysMode { get; set; }

        [Option("labels", Required = true, HelpText = "Suitable labels for issue days incrementation")]
        public virtual string Labels { get; set; }

        [Option("since", Required = false, HelpText = "Only issues updated during this last number of days are processed (none restriction by default)")]
        public virtual int? DaysSpan { get; set; }

        [Option("auto-cleanup", Required = false, Default = false, HelpText = "Cleanup labels from issues in closed state (false by default)")]
        public virtual bool AutoCleanup { get; set; }

        [Option("github-token", Required = true, HelpText = "Github secret token")]
        public virtual string GithubToken { get; set; }

        [Option("label-to-increment",
            Required = true,
            HelpText = @"Label that will be added to issue, for example ""In progress: {0} days"", placeholder is required")]
        public virtual string LabelToIncrement { get; set; }
    }
}

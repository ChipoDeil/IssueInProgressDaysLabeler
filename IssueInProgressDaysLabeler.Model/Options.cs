using System.Collections.Generic;
using CommandLine;

namespace IssueInProgressDaysLabeler.Model
{
    /// <summary>
    /// This represents the parameters entity for the console app.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Repository name and owner
        /// </summary>
        [Option("github-repository", Required = true, HelpText = "Repository to increment days in and owner")]
        public virtual string GithubRepositoryName { get; set; }

        /// <summary>
        /// Days on which action will increment value
        /// </summary>
        [Option("days-mode", Required = true, HelpText = "Enum: EveryDay, EveryDayExceptWeekend, RussianCalendar")]
        public virtual DaysMode DaysMode { get; set; }

        /// <summary>
        /// Suitable labels for issue days incrementation
        /// </summary>
        [Option("labels", Required = true, HelpText = "Suitable labels for issue days incrementation")]
        public virtual string Labels { get; set; }

        /// <summary>
        /// Github secret token
        /// </summary>
        [Option("github-token", Required = true, HelpText = "Github secret token")]
        public virtual string GithubToken { get; set; }
    }
}

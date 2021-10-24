using System.Collections.Generic;
using CommandLine;

namespace IssueInProgressDaysLabeler.Model
{
    /// <summary>
    /// This represents the parameters entity for the console app.
    /// </summary>
    internal class Options
    {
        /// <summary>
        /// Project to increment days in
        /// </summary>
        [Option("owner", Required = true, HelpText = "Owner of repository")]
        internal virtual string Owner { get; set; }

        /// <summary>
        /// Repository name
        /// </summary>
        [Option("repository-name", Required = true, HelpText = "Repository to increment days in")]
        internal virtual string Repository { get; set; }

        /// <summary>
        /// Suitable labels for issue days incrementation
        /// </summary>
        [Option("labels", Required = true, HelpText = "Suitable labels for issue days incrementation")]
        internal virtual IReadOnlyCollection<string> Labels { get; set; }

        /// <summary>
        /// Github secret token
        /// </summary>
        [Option("github-token", Required = true, HelpText = "Github secret token")]
        internal virtual string GithubToken { get; set; }
    }
}

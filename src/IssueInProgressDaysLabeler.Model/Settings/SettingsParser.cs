using System;
using CommandLine;
using IssueInProgressDaysLabeler.Model.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IssueInProgressDaysLabeler.Model.Settings
{
    internal class SettingsParser : ISettingsParser
    {
        private readonly ILogger<SettingsParser> _logger;

        public SettingsParser(ILogger<SettingsParser> logger)
        {
            _logger = logger;
        }

        public IssueInProgressConsoleSettings? TryParse(string[] programArgs)
        {
            _logger.Log(LogLevel.Information, JsonConvert.SerializeObject(programArgs));

            using var parser = new Parser(static with =>
            {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out; // TODO: custom text writer (?)
            });

            var result = parser.ParseArguments<Options>(programArgs)
                .MapResult(OnParsed, static _ => default);

            return result;
        }

        private static IssueInProgressConsoleSettings? OnParsed(Options options)
        {
            var splitItems = options.GithubRepositoryName.Split('/');
            var owner = splitItems[0];
            var repository = splitItems[1];

            if(!options.LabelToIncrement.Contains(LabelerConstants.RequiredPlaceholder))
                throw new ArgumentException("LabelToIncrement: placeholder required");

            var autoCleanup = !string.IsNullOrEmpty(options.AutoCleanup) && bool.Parse(options.AutoCleanup);

            var since = string.IsNullOrEmpty(options.DaysSince)
                ? default(DateTimeOffset?)
                : DateTimeOffset.UtcNow - TimeSpan.FromDays(int.Parse(options.DaysSince));

            var labels = JsonConvert.DeserializeObject<string[]>(options.Labels);

            if (labels == null)
                throw new InvalidOperationException("labels can not be deserialized");

            return new(
                owner,
                repository,
                labels,
                options.GithubToken,
                Enum.Parse<DaysMode>(options.DaysMode),
                options.LabelToIncrement,
                since,
                autoCleanup);
        }
    }
}
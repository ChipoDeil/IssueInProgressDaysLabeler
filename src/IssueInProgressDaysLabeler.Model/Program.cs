﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using IssueInProgressDaysLabeler.Model.IssueUpdateStrategies;
using Newtonsoft.Json;

namespace IssueInProgressDaysLabeler.Model
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            Console.WriteLine(JsonConvert.SerializeObject(args));

            using var parser = new Parser(with =>
            {
                with.EnableDashDash = true;
                with.HelpWriter = Console.Out;
            });

            var result = parser.ParseArguments<Options>(args)
                    .MapResult(OnParsed, OnError);

            return result;
        }

        private static async Task<int> OnParsed(Options options)
        {
            var settings = ParseSettings(options);

            Console.WriteLine($"Labels: {JsonConvert.SerializeObject(settings.Labels)}");

            var strategies = IssueUpdateStrategyFactory.Create(settings);

            var gitHubClientFacade = GithubClientFactory.Create(
                settings.GithubToken,
                settings.Owner,
                settings.Repository);

            var issuesToUpdate = await gitHubClientFacade
                .GetIssuesToUpdate(settings.Labels, settings.Since);

            foreach (var issue in issuesToUpdate)
            {
                foreach (var strategy in strategies)
                {
                    strategy.TryUpdateIssue(issue);
                }
            }

            await Task.WhenAll(gitHubClientFacade.UpdateIssues(issuesToUpdate));

            return StatusCodeConstants.SuccessStatusCode;
        }

        private static Task<int> OnError(IEnumerable<Error> errors)
            => Task.FromResult(StatusCodeConstants.ErrorBadArgumentsStatusCode);

        private static Settings ParseSettings(Options options)
        {
            var splitItems = options.GithubRepositoryName.Split('/');
            var owner = splitItems[0];
            var repository = splitItems[1];

            if(!options.LabelToIncrement.Contains(LabelerConstants.RequiredPlaceholder))
                throw new ArgumentException("LabelToIncrement: placeholder required");

            var autoCleanup = !string.IsNullOrEmpty(options.AutoCleanup) && bool.Parse(options.AutoCleanup);

            var since = string.IsNullOrEmpty(options.DaysSince)
                ? (DateTimeOffset?) null
                : DateTimeOffset.UtcNow - TimeSpan.FromDays(int.Parse(options.DaysSince));

            return new(
                owner,
                repository,
                labels: JsonConvert.DeserializeObject<string[]>(options.Labels),
                options.GithubToken,
                daysMode: Enum.Parse<DaysMode>(options.DaysMode),
                options.LabelToIncrement,
                since,
                autoCleanup);
        }
    }
}
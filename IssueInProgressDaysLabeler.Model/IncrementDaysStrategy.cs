using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mindbox.WorkingCalendar;
using Newtonsoft.Json;

namespace IssueInProgressDaysLabeler.Model
{
    internal static class IncrementDaysStrategy
    {
        internal static async Task IncrementDays(
            GithubClientFacade githubClientFacade,
            IReadOnlyCollection<string> labels)
        {
            var workingCalendar = new WorkingCalendar(new Dictionary<DateTime, DayType>());

            if (!workingCalendar.IsWorkingDay(DateTime.UtcNow)) return;

            var issuesToUpdate = await githubClientFacade
                .GetIssuesToUpdate(labels);

            Console.WriteLine(JsonConvert.SerializeObject(issuesToUpdate));

            var issuesToAddLabel = issuesToUpdate
                .Where(i => i.IssueUpdate.Labels.Any(l =>
                    l.StartsWith(GithubConstants.LabelTemplatePrefix)))
                .ToArray();

            foreach (var issueToAddLabel in issuesToAddLabel)
            {
                issueToAddLabel.IssueUpdate.Labels.Add(GetLabelForDays(1));
            }

            var issuesToIncrement = issuesToUpdate.Except(issuesToAddLabel).ToArray();

            foreach (var issueToIncrement in issuesToIncrement.Select(c => c.IssueUpdate))
            {
                var labelToRemove = issueToIncrement.Labels.Single(c =>
                    c.StartsWith(GithubConstants.LabelTemplatePrefix));
                issueToIncrement.Labels.Remove(labelToRemove);

                var regex = new Regex("\\d+");

                var currentDays = int.Parse(regex.Match(labelToRemove).Groups.Values.Single().Value);

                issueToIncrement.AddLabel(GetLabelForDays(currentDays + 1));
            }

            await Task.WhenAll(githubClientFacade.UpdateIssues(issuesToUpdate));

            static string GetLabelForDays(int days) => string.Format(GithubConstants.LabelTemplate, days);
        }
    }
}
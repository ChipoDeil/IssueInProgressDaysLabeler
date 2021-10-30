using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IssueInProgressDaysLabeler.Model
{
    internal static class IncrementDaysStrategy
    {
        internal static async Task IncrementDays(
            GithubClientFacade githubClientFacade,
            IDaysModeHelper daysModeHelper,
            IReadOnlyCollection<string> labels,
            string labelToIncrement)
        {
            const string number = "\\d+";

            if (!daysModeHelper.IsSuitableDay(DateTime.UtcNow)) return;

            var issuesToUpdate = await githubClientFacade
                .GetIssuesToUpdate(labels);

            Console.WriteLine(JsonConvert.SerializeObject(issuesToUpdate));

            if (!issuesToUpdate.Any()) return;

            var labelRegex = new Regex(string.Format(labelToIncrement, number));
            var issuesToAddLabel = issuesToUpdate
                .Where(i => !i.IssueUpdate.Labels.Any(l => labelRegex.IsMatch(l)))
                .ToArray();

            foreach (var issueToAddLabel in issuesToAddLabel)
            {
                issueToAddLabel.IssueUpdate.Labels.Add(GetLabelForDays(1));
            }

            var issuesToIncrement = issuesToUpdate.Except(issuesToAddLabel).ToArray();

            foreach (var issueToIncrement in issuesToIncrement.Select(c => c.IssueUpdate))
            {
                var labelToRemove = issueToIncrement.Labels.First(c => labelRegex.IsMatch(c));
                issueToIncrement.Labels.Remove(labelToRemove);

                var numberParseRegex = new Regex(number);

                var currentDays = int.Parse(numberParseRegex.Match(labelToRemove).Groups.Values.Single().Value);

                issueToIncrement.AddLabel(GetLabelForDays(currentDays + 1));
            }

            await Task.WhenAll(githubClientFacade.UpdateIssues(issuesToUpdate));

            string GetLabelForDays(int days) => string.Format(labelToIncrement, days);
        }
    }
}
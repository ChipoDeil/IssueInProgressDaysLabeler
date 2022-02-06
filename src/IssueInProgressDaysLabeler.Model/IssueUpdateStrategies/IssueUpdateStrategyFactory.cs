using System;
using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies
{
    internal static class IssueUpdateStrategyFactory
    {
        internal static IReadOnlyCollection<IssueUpdateStrategy> Create(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var strategies = new List<IssueUpdateStrategy>
            {
                new IncrementDaysStrategy(new DaysModeHelper(settings.DaysMode), settings.LabelToIncrement)
            };

            if (settings.AutoCleanup)
                strategies.Add(new CleanUpLabelStrategy(settings.LabelToIncrement));

            return strategies;
        }
    }
}
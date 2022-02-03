using System;
using System.Collections.Generic;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies
{
    internal static class IssueUpdateStrategyFactory
    {
        internal static SortedSet<IssueUpdateStrategy> Create(Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            SortedSet<IssueUpdateStrategy> strategies = new()
            {
                new IncrementDaysStrategy(new DaysModeHelper(settings.DaysMode), settings.LabelToIncrement)
            };

            if (settings.AutoCleanup)
                strategies.Add(new CleanUpLabelStrategy(settings.LabelToIncrement));

            return strategies;
        }
    }
}
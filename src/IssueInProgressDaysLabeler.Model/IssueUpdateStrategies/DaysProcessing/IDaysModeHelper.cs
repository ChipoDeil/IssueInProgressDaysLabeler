using System;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies.DaysProcessing
{
    internal interface IDaysModeHelper
    {
        bool IsSuitableDay(DateTime dateTime);
    }
}
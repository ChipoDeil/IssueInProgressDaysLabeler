using System;

namespace IssueInProgressDaysLabeler.Model
{
    internal interface IDaysModeHelper
    {
        bool IsSuitableDay(DateTime dateTime);
    }
}
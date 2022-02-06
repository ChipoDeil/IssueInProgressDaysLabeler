using System;

namespace IssueInProgressDaysLabeler.Model.Extensions
{
    internal static class DateTimeExtensions
    {
        internal static bool IsWorkingDay(this DateTime dateTime)
        {
            return dateTime.DayOfWeek != DayOfWeek.Sunday && dateTime.DayOfWeek != DayOfWeek.Saturday;
        }
    }
}
using System;
using System.Collections.Generic;
using IssueInProgressDaysLabeler.Model.Extensions;
using IssueInProgressDaysLabeler.Model.Settings;
using Mindbox.WorkingCalendar;

namespace IssueInProgressDaysLabeler.Model.IssueUpdateStrategies.DaysProcessing
{
    internal class DaysModeHelper : IDaysModeHelper
    {
        private readonly WorkingCalendar _workingCalendar;
        private readonly DaysMode _daysMode;

        public DaysModeHelper(DaysModeHelperSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            _daysMode = settings.DaysMode;
            _workingCalendar = new WorkingCalendar(new Dictionary<DateTime, DayType>());
        }

        public bool IsSuitableDay(DateTime dateTime)
        {
            switch (_daysMode)
            {
                case DaysMode.EveryDay:
                    return true;
                case DaysMode.EveryDayExceptWeekend:
                    return dateTime.IsWorkingDay();
                case DaysMode.RussianCalendar:
                    return _workingCalendar.IsWorkingDay(dateTime);
                default:
                    throw new ArgumentOutOfRangeException(nameof(_daysMode));
            }
        }
    }
}
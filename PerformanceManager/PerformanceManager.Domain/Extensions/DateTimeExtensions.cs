using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Shared;

namespace PerformanceManager.Domain.Extensions
{
    /// <summary>
    /// Provides extension methods for the System.DateTime class.
    /// </summary>
    public static class DateTimeExtensions
    {


        /// <summary>
        /// Calculates the next business day from the given date.
        /// </summary>
        /// <param name="fromDate">Current date or the start date from which the next business day has to be calculated.</param>
        /// <returns>A <see cref="DateTime"/> object representing the next business/working day.</returns>
        public static DateTime NextBusinessDay(this DateTime fromDate)
        {
            int daysToAdd = fromDate.DayOfWeek == DayOfWeek.Friday ? 3 :
                            fromDate.DayOfWeek == DayOfWeek.Saturday ? 2 : 1;

            return fromDate.AddDays(daysToAdd);
        }
        /// <summary>
        /// Calculates the next business day from the given date considering a list of holidays.
        /// </summary>
        /// <param name="fromDate">Current date or the start date from which the next business day has to be calculated.</param>
        /// <param name="holidays">A list of holidays prepopulated for this year.</param>
        /// <returns>A <see cref="DateTime"/> object representing the next business/working day.</returns>
        public static DateTime NextBusinessDay(this DateTime fromDate, HashSet<DateTime> holidays)
        {

            DateTime nextBusinessDay = fromDate.NextBusinessDay();

            if (holidays != null && holidays.Count > 1)
            {
                HashSet<DateTime> holidaysToAvail = holidays.Where(d => d > fromDate).ToHashSet();

                while (holidaysToAvail.Count > 1 && holidaysToAvail.Contains(nextBusinessDay))
                {
                    nextBusinessDay = nextBusinessDay.NextBusinessDay();
                }
            }

            return nextBusinessDay;
        }
        /// <summary>
        /// Calculates the next business day from the given date considering a list of holidays and after a specified number of days.
        /// </summary>
        /// <param name="fromDate">Current date or the start date from which the next business day has to be calculated.</param>
        /// <param name="holidays">A list of holidays prepopulated for this year.</param>
        /// <param name="afterDays">Number of working/business days after which the business day to be calculated.</param>
        /// <returns>A <see cref="DateTime"/> object representing the next business/working day.</returns>
        public static DateTime NextBusinessDay(this DateTime fromDate, HashSet<DateTime> holidays, int afterDays)
        {

            DateTime nextBusinessDay = fromDate;

            for (int i = 0; i < afterDays; i++)
            {
                nextBusinessDay = NextBusinessDay(nextBusinessDay, holidays);
            }

            return nextBusinessDay;
        }
        /// <summary>
        ///  Calculates the next business day from the given date considering a list of holidays and after a specified number of working hours.
        /// </summary>
        /// <param name="fromDate">Current date or the start date from which the next business day has to be calculated.</param>
        /// <param name="totalWorkHours">Number of working/business hours after which the business day to be calculated.</param>
        /// <param name="holidays">A list of holidays prepopulated for this year.</param>
        /// <param name="workHoursPerDay">Number of working hours per day on a working day.</param>
        /// <returns>A <see cref="DateTime"/> object representing the next business/working day.</returns>
        public static DateTime NextBusinessDay(this DateTime fromDate, int totalWorkHours, HashSet<DateTime> holidays = null, int workHoursPerDay = 9)
        {
            int totalWorkDays = MathExtensions.CeilingTo(totalWorkHours / workHoursPerDay, 1);

            return NextBusinessDay(fromDate, holidays, totalWorkDays);
        }
        /// <summary>
        ///  Calculates the next business day along with time from the given date by taking the list of holidays in to and after a specified number of working hours.
        ///  This provides the timing based considering the work starting hour as passed to the method.
        /// </summary>
        /// <param name="fromDate">Current date or the start date from which the next business day has to be calculated.</param>
        /// <param name="totalWorkHours">Number of working/business hours after which the business day to be calculated.</param>
        /// <param name="holidays">A list of holidays prepopulated for this year.</param>
        /// <param name="workHoursPerDay">Number of working hours per day on a working day.</param>
        /// <param name="workStartHour">Hour at which the work starts on a working day.</param>
        /// <returns>A <see cref="DateTime"/> object representing the next business/working day.</returns>
        public static DateTime NextBusinessHour(this DateTime fromDate, double totalWorkHours, HashSet<DateTime> holidays = null, double workHoursPerDay = 9, double workStartHour = 9)
        {
            TimeSpan timeSpanFromGivenDate = fromDate.ExtractTimeSpan();
            TimeSpan timeSpanFromWorkStartHour = TimeSpan.FromHours(workStartHour);

            int wholeWorkDays = (int)MathExtensions.FloorTo((decimal)(totalWorkHours / workHoursPerDay), 1);

            DateTime nextBusinessDay = NextBusinessDay(fromDate, holidays, wholeWorkDays);

            double fractionalHoursLeft = ((totalWorkHours / workHoursPerDay) - wholeWorkDays) * workHoursPerDay;

            if (timeSpanFromGivenDate > timeSpanFromWorkStartHour)
            {
                TimeSpan timeSpanRemainingOnTheGivenDate = fromDate.RemainingTimeSpanInTheWorkDay(workHoursPerDay, workStartHour);

                if (timeSpanRemainingOnTheGivenDate.TotalHours >= fractionalHoursLeft)
                {
                    nextBusinessDay = nextBusinessDay.AddHours(fractionalHoursLeft);
                }
                else
                {
                    nextBusinessDay = nextBusinessDay.NextBusinessDay().AddHours(fractionalHoursLeft-timeSpanRemainingOnTheGivenDate.TotalHours);
                }
            }
            else
            {
                nextBusinessDay = nextBusinessDay.ResetTimeSpanToZero().AddHours(workStartHour + (fractionalHoursLeft > 0 ? fractionalHoursLeft : workHoursPerDay));
            }

            return nextBusinessDay;
        }

        /// <summary>
        /// Extracts the hours, minutes, seconds and milliseconds and return a new <see cref="TimeSpan"/> object from the given date object.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> object from which the new <see cref="TimeSpan"/> object to be extracted.</param>
        /// <returns>A <see cref="TimeSpan"/> object from the given <see cref="DateTime"/> object.</returns>
        public static TimeSpan ExtractTimeSpan(this DateTime dateTime) => new(0, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond);
        /// <summary>
        /// Resets the time span to zero for the given <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="dateTime">A <see cref="DateTime"/> object.</param>
        /// <returns>A <see cref="DateTime"/> object with it's <see cref="TimeSpan"/> set to zero.</returns>
        public static DateTime ResetTimeSpanToZero(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, dateTime.Day);
        /// <summary>
        /// Calculates the remaining in a given working day considering the input time as the start time.
        /// </summary>
        /// <param name="sourceDate">A <see cref="DateTime"/> object indicating the date and time.</param>
        /// <param name="workHoursPerDay">Total available working hours in a working day.</param>
        /// <param name="workStartHour">Time at which a working day starts.</param>
        /// <returns>A <see cref="TimeSpan"/> object indicating the total time left in the given working day.</returns>
        public static TimeSpan RemainingTimeSpanInTheWorkDay(this DateTime sourceDate, double workHoursPerDay = 9, double workStartHour = 9)
        {
            TimeSpan sourceDateTimeSpan = sourceDate.ExtractTimeSpan();

            TimeSpan workDayEndTimeSpan = TimeSpan.FromHours(workHoursPerDay) + TimeSpan.FromHours(workStartHour);

            TimeSpan workDayRemainingTimeSpan = workDayEndTimeSpan - sourceDateTimeSpan;

            return workDayRemainingTimeSpan;

        }

    }
}

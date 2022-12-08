using System;
using System.Collections.Generic;

using SlvParkview.FinanceManager.Utilities;

namespace SlvParkview.FinanceManager.Services
{
    public static class DayOccurencesFinder
    {
        public static List<DateTime> FindFor(DateTime startDate, DateTime endDate, int dayOfMonth)
        {
            if (endDate < startDate)
            {
                throw new ArgumentException($"Invalid {nameof(startDate)} or {nameof(endDate)} argument. {nameof(startDate)} shall be less than {nameof(endDate)}.");
            }
            if (dayOfMonth < 0 || dayOfMonth > 31)
            {
                throw new ArgumentOutOfRangeException($"Invalid {nameof(dayOfMonth)} argument. The value shall be between 1 and 31 both inclusive.");
            }

            List<DateTime> dates = new List<DateTime>();

            int nYears = endDate.Year - startDate.Year;

            /// If the number of years (difference in end and start year) is zero, it indicates that the range of dates are in
            /// the same year and hence <see cref="PartialYear"/> shall be used.
            if (nYears == 0)
            {
                dates = PartialYear.GetDateOccurences(startDate, endDate, dayOfMonth);
            }
            /// If the number of years is equal to or greater than 1, it indicates either:
            ///       a. nYears = 1: The dates range is spanning across two adjacent years.
            ///       b. nYears > 1: There will be two <see cref="PartialYear"/> at the beginning and ending, and 
            ///                      one or more complete years between the given date range.
            else
            {
                /// Add dates from the beginning <see cref="PartialYear"/>.
                dates.AddRange(PartialYear.GetDateOccurences(startDate, new DateTime(startDate.Year, 12, 31), dayOfMonth));

                /// Add dates from the <see cref="CompleteYear"/> between the given date range.
                for (int yearCount = 1; yearCount < nYears; yearCount++)
                {
                    dates.AddRange(CompleteYear.GetDateOccurences(startDate.Year + yearCount, dayOfMonth));
                }

                /// Add dates from the ending <see cref="PartialYear"/>.
                dates.AddRange(PartialYear.GetDateOccurences(new DateTime(endDate.Year, 01, 01), endDate, dayOfMonth));
            }

            return dates;

        }
    }
}

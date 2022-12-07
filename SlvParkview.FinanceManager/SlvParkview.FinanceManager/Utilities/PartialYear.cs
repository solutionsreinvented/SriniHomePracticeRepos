using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Utilities
{
    public class PartialYear
    {
        /// <summary>
        /// Generates a <see cref="List{Date}"/> indicating the occurence of a specified day between the given range of dates.
        /// </summary>
        /// <param name="startDate">The start date from which the occurences to be checked.</param>
        /// <param name="endDate">The end date for the verification of occurences.</param>
        /// <param name="dayOfMonth">Indicates the day of the month, the occurence of which has to be checked.</param>
        /// <returns>A <see cref="List{DateTime}"/> of dates.</returns>
        public static List<DateTime> GetDateOccurences(DateTime startDate, DateTime endDate, int dayOfMonth)
        {

            List<DateTime> dates = new List<DateTime>();

            /// The day occurs only in the following different cases:
            ///     a. Start date and the end dates are same.                          | Date cccurences: 1
            if (startDate == endDate)
            {
                if (startDate.Day == dayOfMonth)
                {
                    dates.Add(new DateTime(startDate.Year, startDate.Month, dayOfMonth));
                }
            }
            ///     b. Start date is less than the specified date in the start month.  | Date cccurences: 1 (or 0)
            else if (startDate.Day <= dayOfMonth)
            {
                if (startDate.Month == endDate.Month)
                {
                    if (dayOfMonth <= endDate.Day)
                    {
                        dates.Add(new DateTime(startDate.Year, startDate.Month, dayOfMonth));
                    }
                }
                else
                {
                    dates.Add(new DateTime(startDate.Year, startDate.Month, dayOfMonth));
                }
            }

            ///     c. Number of months between the end and start date is more than 1. | Date occurences: nMonths (- 1) + b
            for (int month = startDate.Month + 1; month <= endDate.Month; month++)
            {
                /// If the month is equal to the end month
                if (month == endDate.Month)
                {
                    /// and the specified date is less than the day of the end date, it counts.
                    if (dayOfMonth <= endDate.Day)
                    {
                        dates.Add(new DateTime(endDate.Year, month, dayOfMonth));
                    }
                }
                /// Otherwise, it is still between the given dates range and hence it counts.
                else
                {
                    dates.Add(new DateTime(startDate.Year, month, dayOfMonth));
                }
            }

            return dates;
        }
    }
}

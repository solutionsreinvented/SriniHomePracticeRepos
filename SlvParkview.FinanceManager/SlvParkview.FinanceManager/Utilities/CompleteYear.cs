using System;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.Utilities
{
    public class CompleteYear
    {
        /// <summary>
        /// Generates a <see cref="List{Date}"/> indicating the occurence of a specified day in a given year.
        /// </summary>
        /// <param name="year">The year in which the dates to be checked for.</param>
        /// <param name="dayOfMonth">Indicates the day of the month, the occurence of which has to be checked.</param>
        /// <returns>A <see cref="List{DateTime}"/> of dates.</returns>
        public static List<DateTime> GetDateOccurences(int year, int dayOfMonth)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            return PartialYear.GetDateOccurences(startDate, endDate, dayOfMonth);
        }
    }
}

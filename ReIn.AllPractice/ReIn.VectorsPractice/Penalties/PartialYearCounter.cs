using System;
using System.Collections.Generic;

namespace ReIn.VectorsPractice.Models
{
    public class PartialYearCounter
    {
        public static List<DateTime> GetCount(DateTime startDate, DateTime endDate, int dayOfMonth)
        {
            int counter = 0;
            List<DateTime> dates = new List<DateTime>();

            if (startDate == endDate)
            {
                if (startDate.Day == dayOfMonth)
                {
                    counter++;
                    dates.Add(new DateTime(startDate.Year, startDate.Month, dayOfMonth));
                }
            }
            else if (startDate.Day <= dayOfMonth)
            {
                counter++;
                dates.Add(new DateTime(startDate.Year, startDate.Month, dayOfMonth));
            }

            for (int month = startDate.Month + 1; month <= endDate.Month; month++)
            {
                if (month == endDate.Month)
                {
                    if (dayOfMonth <= endDate.Day)
                    {
                        counter++;
                        dates.Add(new DateTime(endDate.Year, month, dayOfMonth));
                    }
                }
                else
                {
                    counter++;
                    dates.Add(new DateTime(startDate.Year, month, dayOfMonth));
                }
            }

            return dates;
        }
    }
}

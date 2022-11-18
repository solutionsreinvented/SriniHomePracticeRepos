using System;
using System.Collections.Generic;

namespace ReIn.DateTimePractice
{
    public static class DayOccurenceFinder
    {
        private static void AddDate(List<DateTime> dates, int year, int month, int day)
        {
            if (day <= DateTime.DaysInMonth(year, month))
            {
                dates.Add(new DateTime(year, month, day));
            }
        }

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

            for (int year = startDate.Year; year <= endDate.Year; year++)
            {
                if (startDate.Year == endDate.Year)
                {
                    if (startDate.Day <= dayOfMonth)
                    {
                        AddDate(dates, year, startDate.Month, dayOfMonth);

                        //if (dayOfMonth <= DateTime.DaysInMonth(year, startDate.Month))
                        //{
                        //    dates.Add(new DateTime(year, startDate.Month, dayOfMonth));
                        //}
                    }

                    for (int month = startDate.Month + 1; month < endDate.Month; month++)
                    {
                        AddDate(dates, year, month, dayOfMonth);
                        
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, month))
                        //{
                        //    dates.Add(new DateTime(year, month, dayOfMonth));
                        //}
                    }

                    if (dayOfMonth <= endDate.Day && startDate != endDate)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, endDate.Month))
                        //{
                        //    dates.Add(new DateTime(year, endDate.Month, dayOfMonth));
                        //}

                        AddDate(dates, year, endDate.Month, dayOfMonth);
                    }

                }
                else if (year == startDate.Year)
                {
                    if (startDate.Day <= dayOfMonth)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, startDate.Month))
                        //{
                        //    dates.Add(new DateTime(year, startDate.Month, dayOfMonth));
                        //}

                        AddDate(dates, year, startDate.Month, dayOfMonth);
                    }

                    for (int month = startDate.Month + 1; month <= 12; month++)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, month))
                        //{
                        //    dates.Add(new DateTime(year, month, dayOfMonth));
                        //}

                        AddDate(dates, year, month, dayOfMonth);
                    }
                }
                else if (year == endDate.Year)
                {
                    for (int month = 1; month < endDate.Month; month++)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, month))
                        //{
                        //    dates.Add(new DateTime(year, month, dayOfMonth));
                        //}

                        AddDate(dates, year, month, dayOfMonth);
                    }

                    if (dayOfMonth <= endDate.Day)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, endDate.Month))
                        //{
                        //    dates.Add(new DateTime(year, endDate.Month, dayOfMonth));
                        //}

                        AddDate(dates, year, endDate.Month, dayOfMonth);
                    }
                }
                else
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        //if (dayOfMonth <= DateTime.DaysInMonth(year, month))
                        //{
                        //    dates.Add(new DateTime(year, month, dayOfMonth));
                        //}

                        AddDate(dates, year, month, dayOfMonth);
                    }
                }

            }

            return dates;

        }
    }
}

using ActivityTracker.Domain.Interfaces;
using ActivityTracker.Domain.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivityTracker.Domain.Base
{
    public abstract class Year : IYear
    {
        protected readonly int _year;

        protected static readonly HashSet<DateTime> _holidays = new HolidaysRepository().GetAll();

        protected Year(int year)
        {
            _year = year;
        }

        public virtual DateTime StartDate { get; }

        public virtual DateTime EndDate { get; }

        public CategorizedDays CategorizedDays => CategorizeDaysBetween(StartDate, EndDate);

        public static CategorizedDays CategorizeDaysBetween(DateTime from, DateTime to)
        {
            int differenceDays = (int)to.Subtract(from).TotalDays + 1;

            CategorizedDays categorizedDays = new();

            IEnumerable<DateTime> allDaysBetweenGivenDates = Enumerable
                                                                      .Range(0, differenceDays)
                                                                      .Select(x => from.AddDays(x));

            categorizedDays.Holidays = allDaysBetweenGivenDates
                                                       .Where(d => _holidays.Contains(d));

            categorizedDays.WeekEnds = allDaysBetweenGivenDates
                                                       .Where(d => d.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday);

            categorizedDays.WorkDays = allDaysBetweenGivenDates
                                                       .Except(categorizedDays.Holidays)
                                                       .Except(categorizedDays.WeekEnds);


            return categorizedDays;
        }
    }

    public struct CategorizedDays
    {
        public CategorizedDays(IEnumerable<DateTime> workdays,
                               IEnumerable<DateTime> holidays,
                               IEnumerable<DateTime> weekends)
        {
            WorkDays = workdays;
            Holidays = holidays;
            WeekEnds = weekends;
        }

        public IEnumerable<DateTime> WorkDays { get; set; }

        public IEnumerable<DateTime> Holidays { get; set; }

        public IEnumerable<DateTime> WeekEnds { get; set; }
    }

}

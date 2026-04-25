using System;
using System.Collections.Generic;
using System.Linq;

using ProdActivity.Domain.Interfaces;
using ProdActivity.Domain.Models;
using ProdActivity.Domain.Data.Entities;

namespace ProdActivity.Domain.Services
{
    public class DateCalculationService
    {
        private readonly HashSet<DateTime> _holidays;
        public double WorkingHoursPerDay { get; set; } = 8.0;

        public DateCalculationService(IEnumerable<DbHoliday> holidays)
        {
            _holidays = new HashSet<DateTime>(holidays.Select(h => h.Date.Date));
        }

        public bool IsWorkingDay(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            if (_holidays.Contains(date.Date))
                return false;

            return true;
        }

        public DateTime GetNextWorkingDay(DateTime startDate)
        {
            DateTime nextDay = startDate.Date.AddDays(1);
            while (!IsWorkingDay(nextDay))
            {
                nextDay = nextDay.AddDays(1);
            }
            return nextDay;
        }

        public DateTime CalculateCompletionDate(DateTime startDate, double requiredHours)
        {
            if (requiredHours <= 0) return startDate;

            double remainingHours = requiredHours;
            DateTime currentDate = startDate.Date;

            // If the start date is not a working day, move to the next working day
            if (!IsWorkingDay(currentDate))
            {
                currentDate = GetNextWorkingDay(currentDate);
            }

            while (remainingHours > WorkingHoursPerDay)
            {
                remainingHours -= WorkingHoursPerDay;
                currentDate = GetNextWorkingDay(currentDate);
            }

            return currentDate;
        }
        
        public double CalculateRequiredHours(DateTime startDate, DateTime completionDate)
        {
            double totalHours = 0;
            DateTime currentDate = startDate.Date;

            while (currentDate <= completionDate.Date)
            {
                if (IsWorkingDay(currentDate))
                {
                    totalHours += WorkingHoursPerDay;
                }
                currentDate = currentDate.AddDays(1);
            }

            return totalHours;
        }
    }
}

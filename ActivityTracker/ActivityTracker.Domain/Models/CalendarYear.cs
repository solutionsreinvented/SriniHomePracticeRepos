using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Interfaces;

using System;

namespace ActivityTracker.Domain.Models
{
    public class CalendarYear : Year, IYear
    {

        public CalendarYear(int year) : base(year)
        {

        }

        public override DateTime StartDate => new(_year, 01, 01);

        public override DateTime EndDate => new(_year, 12, 31);
    }
}

using ActivityTracker.Domain.Base;
using ActivityTracker.Domain.Interfaces;

using System;

namespace ActivityTracker.Domain.Models
{
    public class FinancialYear : Year, IYear
    {

        public FinancialYear(int year) : base(year)
        {

        }

        public override DateTime StartDate => new(_year, 04, 01);

        public override DateTime EndDate => new(_year + 1, 03, 31);
    }
}

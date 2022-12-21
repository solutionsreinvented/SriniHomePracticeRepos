using PerformanceManager.Domain.Base;
using PerformanceManager.Domain.Interfaces;

using System;

namespace PerformanceManager.Domain.Models
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

using ProdActivity.Domain.Base;
using ProdActivity.Domain.Interfaces;

using System;

namespace ProdActivity.Domain.Models
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

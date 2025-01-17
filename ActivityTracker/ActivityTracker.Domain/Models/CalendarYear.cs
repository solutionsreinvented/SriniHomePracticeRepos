using ProdActivity.Domain.Base;
using ProdActivity.Domain.Interfaces;

using System;

namespace ProdActivity.Domain.Models
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

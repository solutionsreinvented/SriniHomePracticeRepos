using System;
using System.Collections.Generic;

namespace ReIn.VectorsPractice.Models
{
    public class CompleteYearCounter
    {
        public static List<DateTime> GetCount(int year, int dayOfMonth)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);

            return PartialYearCounter.GetCount(startDate, endDate, dayOfMonth);
        }
    }
}

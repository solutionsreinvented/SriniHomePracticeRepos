using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;

using System;

namespace SlvParkview.FinanceManager.Services
{
    public static class ReportsGenerator
    {

        #region Reports

        public static OutstandingsReport GetOutstandingsReport(Flat flat)
        {
            return GetOutstandingsReport(flat, DateTime.Today);
        }

        public static OutstandingsReport GetOutstandingsReport(Flat flat, DateTime generateTill)
        {
            return new OutstandingsReport()
            {
                FlatInfo = PrintableFlat.MapFrom(flat),
                Transactions = flat.GetPrintableTransactions(generateTill)
            };
        }

        #endregion
    }
}

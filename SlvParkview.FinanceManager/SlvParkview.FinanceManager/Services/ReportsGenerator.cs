using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Extensions;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting;
using System;

namespace SlvParkview.FinanceManager.Services
{
    public static class ReportsGenerator
    {

        //public static IReport Generate(ReportType reportType)
        //{
        //    IReport report;

        //    if (reportType == ReportType.FlatTransactionsHistory)
        //    {
        //        report = 
        //    }
        //}


        #region Reports

        public static OverviewReport GetOverviewReport(Block block)
        {
            OverviewReport overviewReport = new OverviewReport();

            if (block != null && block.Flats != null)
            {
                block.Flats.ForEach(f => overviewReport.Flats.Add(f.ParseToFlatInfo()));
            }
            return overviewReport;
        }


        public static FlatTransactionsReport GetFlatTransactionsReport(Flat flat)
        {
            return GetFlatTransactionsReport(flat, DateTime.Today);
        }

        public static FlatTransactionsReport GetFlatTransactionsReport(Flat flat, DateTime generateTill)
        {
            return new FlatTransactionsReport()
            {
                FlatInfo = flat.ParseToFlatInfo(),/// PrintableFlat.MapFrom(flat),
                Transactions = flat.GetTransactionHistory(generateTill)
            };
        }

        #endregion
    }
}

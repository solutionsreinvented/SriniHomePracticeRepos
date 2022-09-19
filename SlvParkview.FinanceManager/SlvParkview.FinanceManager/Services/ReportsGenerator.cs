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

        //public static MonthlyPaymentsReport GetMonthlyPaymentsReport(Block block, int forMonth)
        //{
        //    MonthlyPaymentsReport monthlyPaymentsReport = new MonthlyPaymentsReport() { ReportedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(forMonth) };

        //    block.Flats.ForEach(f => monthlyPaymentsReport.Payments.AddRange(f.Payments.Where(p => p.ReceivedOn.Month == forMonth)) )
        //}

        //public static BlockOustandingsReport GetOverviewReport(Block block)
        //{
        //    BlockOustandingsReport overviewReport = new OverviewReport();

        //    if (block != null && block.Flats != null)
        //    {
        //        block.Flats.ForEach(f => overviewReport.FlatInfoCollection.Add(f.ParseToFlatInfo()));
        //    }
        //    return overviewReport;
        //}


        //public static FlatTransactionsHistoryReport GetFlatTransactionsReport(Flat flat)
        //{
        //    return GetFlatTransactionsReport(flat, DateTime.Today);
        //}

        //public static FlatTransactionsHistoryReport GetFlatTransactionsReport(Flat flat, DateTime generateTill)
        //{
        //    return new FlatTransactionsHistoryReport()
        //    {
        //        FlatInfo = flat.ParseToFlatInfo(),/// PrintableFlat.MapFrom(flat),
        //        Transactions = flat.GetTransactionsHistoryBasic(generateTill)
        //    };
        //}

        #endregion
    }
}

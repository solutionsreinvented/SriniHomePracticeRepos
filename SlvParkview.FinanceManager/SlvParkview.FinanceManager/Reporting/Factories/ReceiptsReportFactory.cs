using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Reporting.Models.Base;

namespace SlvParkview.FinanceManager.Reporting.Factories
{
    public static class ReceiptsReportFactory
    {
        public static Report Create(ReceiptsReportType receiptsReportType, Block block, IReportOptions reportOptions)
        {
            Report receiptsReport;

            switch (receiptsReportType)
            {
                case ReceiptsReportType.Monthwise:
                    receiptsReport = new ReceiptsInAMonthReport(block, reportOptions);
                    break;
                case ReceiptsReportType.ToASelectedDate:
                    receiptsReport = new ReceiptsToASelectedDateReport(block, reportOptions);
                    break;
                case ReceiptsReportType.InADateRange:
                    receiptsReport = new ReceiptsInADateRangeReport(block, reportOptions);
                    break;
                default:
                    receiptsReport = new ReceiptsInAMonthReport(block, reportOptions);
                    break;
            }

            return receiptsReport;
        }
    }
}

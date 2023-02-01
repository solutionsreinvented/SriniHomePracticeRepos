using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Reporting.Models.Base;

namespace SlvParkview.FinanceManager.Reporting.Factories
{
    public static class ReceiptsReportFactory
    {
        public static Report Create(ReceiptsReportType paymentsReportType, Block block, IReportOptions reportOptions)
        {
            Report paymentsReport;

            switch (paymentsReportType)
            {
                case ReceiptsReportType.Monthwise:
                    paymentsReport = new ReceiptsInAMonthReport(block, reportOptions);
                    break;
                case ReceiptsReportType.ToASelectedDate:
                    paymentsReport = new ReceiptsToASelectedDateReport(block, reportOptions);
                    break;
                case ReceiptsReportType.InADateRange:
                    paymentsReport = new ReceiptsInADateRangeReport(block, reportOptions);
                    break;
                default:
                    paymentsReport = new ReceiptsInAMonthReport(block, reportOptions);
                    break;
            }

            return paymentsReport;
        }
    }
}

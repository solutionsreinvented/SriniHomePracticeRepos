using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Reporting.Factories
{
    public static class ReportOptionsFactory
    {
        public static IReportOptions Create(ReceiptsReportType paymentsReportType)
        {
            IReportOptions reportOptions;

            switch (paymentsReportType)
            {
                case ReceiptsReportType.Monthwise:
                    reportOptions = new InAMonthReceiptsReportOptions();
                    break;
                case ReceiptsReportType.ToASelectedDate:
                    reportOptions = new ToASelectedDateReceiptsReportOptions();
                    break;
                case ReceiptsReportType.InADateRange:
                    reportOptions = new DateRangeReceiptsReportOptions();
                    break;
                default:
                    reportOptions = new InAMonthReceiptsReportOptions();
                    break;
            }

            return reportOptions;
        }
    }
}

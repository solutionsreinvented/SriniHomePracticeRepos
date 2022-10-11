using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;

namespace SlvParkview.FinanceManager.Reporting.Factories
{
    public static class ReportOptionsFactory
    {
        public static IReportOptions Create(PaymentsReportType paymentsReportType)
        {
            IReportOptions reportOptions;

            switch (paymentsReportType)
            {
                case PaymentsReportType.Monthwise:
                    reportOptions = new InAMonthPaymentsReportOptions();
                    break;
                case PaymentsReportType.ToASelectedDate:
                    reportOptions = new ToASelectedDatePaymentsReportOptions();
                    break;
                case PaymentsReportType.InADateRange:
                    reportOptions = new DateRangePaymentsReportOptions();
                    break;
                default:
                    reportOptions = new InAMonthPaymentsReportOptions();
                    break;
            }

            return reportOptions;
        }
    }
}

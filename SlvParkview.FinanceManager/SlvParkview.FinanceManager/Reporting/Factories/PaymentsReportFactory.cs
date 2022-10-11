using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Reporting.Models.Base;

namespace SlvParkview.FinanceManager.Reporting.Factories
{
    public static class PaymentsReportFactory
    {
        public static Report Create(PaymentsReportType paymentsReportType, Block block, IReportOptions reportOptions)
        {
            Report paymentsReport;

            switch (paymentsReportType)
            {
                case PaymentsReportType.Monthwise:
                    paymentsReport = new PaymentsInAMonthReport(block, reportOptions);
                    break;
                case PaymentsReportType.ToASelectedDate:
                    paymentsReport = new PaymentsToASelectedDateReport(block, reportOptions);
                    break;
                case PaymentsReportType.InADateRange:
                    paymentsReport = new PaymentsInADateRangeReport(block, reportOptions);
                    break;
                default:
                    paymentsReport = new PaymentsInAMonthReport(block, reportOptions);
                    break;
            }

            return paymentsReport;
        }
    }
}

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.ViewModels;
using SlvParkview.FinanceManager.ViewModels;

namespace SlvParkview.FinanceManager.Factories
{
    /// <summary>
    /// A factory that generates the <see cref="ReportViewModelBase"/> based on the <see cref="ReportType"/> selected.
    /// </summary>
    public static class ReportViewModelFactory
    {
        /// <summary>
        /// Creates a ReportViewModel based on the <see cref="ReportType"/> requested using the <see cref="SummaryViewModel"/>.
        /// </summary>
        /// <param name="summaryViewModel">ViewModel which handles the global data of a specific apartment <see cref="Block"/>.</param>
        /// <param name="reportType">The <see cref="ReportType"/> of the report to be generated.</param>
        /// <returns>A <see cref="ReportViewModelBase"/> generated based on the selected <see cref="ReportType"/>.</returns>
        public static ReportViewModelBase Create(SummaryViewModel summaryViewModel, ReportType reportType)
        {
            ReportViewModelBase reportViewModel;

            switch (reportType)
            {
                case ReportType.FlatTransactionsHistory:
                    reportViewModel = new FlatTransactionsHistoryReportViewModel(summaryViewModel);
                    break;
                case ReportType.BlockOutstandings:
                    reportViewModel = new BlockOutstandingsReportViewModel(summaryViewModel);
                    break;
                case ReportType.PaymentsReceivedInAMonth:
                    reportViewModel = new MonthwisePaymentsReportViewModel(summaryViewModel);
                    break;
                default:
                    reportViewModel = new BlockOutstandingsReportViewModel(summaryViewModel);
                    break;
            }

            return reportViewModel;
        }
    }
}

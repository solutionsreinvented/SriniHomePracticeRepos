using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.ViewModels;

using System;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the summary of total penalties applied on each flat calculated till the specified date.
    /// </summary>
    public class PenaltiesSummaryReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public PenaltiesSummaryReportViewModel(SummaryViewModel summaryViewModel)
            : base(summaryViewModel)
        {

        }

        #endregion

        #region Public Properties

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        public OutstandingsFilter Filter { get => Get(OutstandingsFilter.All); set { Set(value); UpdateReport(); } }

        public string OutstandingHeader { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();
            ReportTill = DateTime.Today;
            ///Filter = OutstandingsFilter.All;
        }

        private void UpdateReport()
        {
            OutstandingHeader = $"Outstanding as on {ReportTill:dd-MMM-yyyy}";
            Report = new BlockOutstandingsReport(Block, ReportTill, Filter);
            Report.GenerateContents();
        }

        #endregion
    }
}

using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.ViewModels;

using System;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the outstandings of each flat on a specified date for a specified block.
    /// </summary>
    public class BlockOutstandingsReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public BlockOutstandingsReportViewModel(SummaryViewModel summaryViewModel)
            : base(summaryViewModel)
        {

        }

        #endregion

        #region Public Properties

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        public string OutstandingHeader { get => Get<string>(); private set => Set(value); }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            ReportTill = DateTime.Today;
        }

        private void UpdateReport()
        {
            OutstandingHeader = $"Outstanding as on {ReportTill:dd-MMM-yyyy}";
            Report = new BlockOutstandingsReport(Block, ReportTill);
            Report.GenerateContents();
        }

        #endregion
    }
}

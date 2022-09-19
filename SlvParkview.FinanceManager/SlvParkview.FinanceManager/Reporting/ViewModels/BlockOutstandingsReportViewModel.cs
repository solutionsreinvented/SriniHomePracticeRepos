using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.ViewModels;

using System;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    public class BlockOutstandingsReportViewModel : ReportViewModelBase
    {

        public BlockOutstandingsReportViewModel(SummaryViewModel summaryViewModel) 
            : base(summaryViewModel)
        {

        }

        private protected override void Initialize()
        {
            base.Initialize();

            ReportTill = DateTime.Today;
        }

        public string OutstandingHeader { get => Get<string>(); private set => Set(value); }

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        private void UpdateReport()
        {
            OutstandingHeader = $"Outstanding as on {ReportTill:dd-MMM-yyyy}";
            Report = new BlockOutstandingsReport(Block, ReportTill);
            Report.GenerateContents();
        }

    }
}

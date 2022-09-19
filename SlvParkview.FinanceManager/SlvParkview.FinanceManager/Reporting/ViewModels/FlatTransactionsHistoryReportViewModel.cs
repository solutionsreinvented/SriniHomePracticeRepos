using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.ViewModels;

using System;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    public class FlatTransactionsHistoryReportViewModel : ReportViewModelBase
    {

        public FlatTransactionsHistoryReportViewModel(SummaryViewModel summaryViewModel) 
            : base(summaryViewModel)
        {

        }

        private protected override void Initialize()
        {
            base.Initialize();

            SelectedFlat = _summaryViewModel.SelectedFlat ?? (Block.Flats?.First());
        }

        public DateTime ReportTill { get => Get(DateTime.Today); set { Set(value); UpdateReport(); } }

        public Flat SelectedFlat { get => Get<Flat>(); set { Set(value); UpdateReport(); } }

        private void UpdateReport()
        {
            Report = new FlatTransactionsHistoryReport(SelectedFlat, ReportTill);
            Report.GenerateContents();
        }
    }
}

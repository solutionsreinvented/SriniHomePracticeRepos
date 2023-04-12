using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.ViewModels;

using System;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// View Model for generating the complete history of transactions of specified flat up to a specified date.
    /// </summary>
    public class FlatTransactionsHistoryReportViewModel : ReportViewModelBase
    {

        #region Parameterized Constructor

        public FlatTransactionsHistoryReportViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService)
            : base(summaryViewModel, navigationService)
        {

        }

        #endregion

        #region Public Properties

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        public Flat SelectedFlat { get => Get<Flat>(); set { Set(value); UpdateReport(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            ReportTill = DateTime.Today;
            SelectedFlat = _summaryViewModel.SelectedFlat ?? (Block.Flats?.First());
        }

        private void UpdateReport()
        {
            Report = new FlatTransactionsHistoryReport(Block, SelectedFlat, ReportTill);
            Report.GenerateContents();
        }

        #endregion
    }
}

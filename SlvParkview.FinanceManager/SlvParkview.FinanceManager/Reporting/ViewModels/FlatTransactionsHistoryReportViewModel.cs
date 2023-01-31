using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
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

        public FlatTransactionsHistoryReportViewModel(SummaryViewModel summaryViewModel)
            : base(summaryViewModel)
        {

        }

        #endregion

        #region Public Properties

        public Flat SelectedFlat { get => Get<Flat>(); set { Set(value); UpdateReport(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            SelectedFlat = _summaryViewModel.SelectedFlat ?? (Block.Flats?.First());
        }

        protected override void UpdateReport()
        {
            Report = new FlatTransactionsHistoryReport(Block, SelectedFlat, ReportTill);
            Report.GenerateContents();
        }

        #endregion
    }
}

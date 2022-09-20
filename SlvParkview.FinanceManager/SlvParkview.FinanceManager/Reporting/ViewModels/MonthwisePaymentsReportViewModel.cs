using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the payments made by one or more flats of a specified <see cref="Block"/> during a specified <see cref="Month"/> and Year.
    /// </summary>
    public class MonthwisePaymentsReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public MonthwisePaymentsReportViewModel(SummaryViewModel summaryViewModel)
            : base(summaryViewModel)
        {

        }

        #endregion

        #region Public Properties

        public IEnumerable<int> Years => Enumerable.Range(2019, 22);

        public int SelectedYear { get => Get<int>(); set { Set(value); UpdateReport(); } }

        public Month SelectedMonth { get => Get<Month>(); set { Set(value); UpdateReport(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            SelectedYear = DateTime.Today.Year;

            SelectedMonth = (Month)DateTime.Today.Month;
        }

        private void UpdateReport()
        {
            Report = new MonthwisePaymentsReport(Block, SelectedMonth, SelectedYear);
            Report.GenerateContents();
        }

        #endregion

    }
}

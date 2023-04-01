using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Models.Base;

using System;
using System.Collections.Generic;
using System.Linq;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class InAMonthReceiptsReportOptions : ReportOptions
    {
        #region Default Constructor

        public InAMonthReceiptsReportOptions() : base()
        {

        }

        #endregion

        #region Public Properties

        public IEnumerable<int> Years => Enumerable.Range(2019, 22);

        public Month SelectedMonth { get => Get<Month>(); set { Set(value); RaiseReportOptionChanged(); } }

        public int SelectedYear { get => Get<int>(); set { Set(value); RaiseReportOptionChanged(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            SelectedMonth = (Month)DateTime.Today.Month;
            SelectedYear = DateTime.Today.Year;
        }

        #endregion
    }
}

using SlvParkview.FinanceManager.Reporting.Models.Base;

using System;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class ToASelectedDateReceiptsReportOptions : ReportOptions
    {
        #region Default Constructor

        public ToASelectedDateReceiptsReportOptions() : base()
        {

        }

        #endregion

        #region Public Properties

        public DateTime SelectedDate { get => Get<DateTime>(); set { Set(value); RaiseReportOptionChanged(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            SelectedDate = DateTime.Today;
        }

        #endregion
    }
}

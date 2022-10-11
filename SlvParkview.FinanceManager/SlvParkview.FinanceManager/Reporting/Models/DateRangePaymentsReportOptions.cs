using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models.Base;

using System;

namespace SlvParkview.FinanceManager.Reporting.Models
{
    public class DateRangePaymentsReportOptions : ReportOptions, IReportOptions
    {
        #region Default Constructor

        public DateRangePaymentsReportOptions() : base()
        {

        }

        #endregion

        #region Public Properties

        public DateTime StartDate { get => Get<DateTime>(); set { Set(value); RaiseReportOptionChanged(); } }

        public DateTime EndDate { get => Get<DateTime>(); set { Set(value); RaiseReportOptionChanged(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            StartDate = DateTime.Today;
            EndDate = DateTime.Today;
        }

        #endregion
    }
}

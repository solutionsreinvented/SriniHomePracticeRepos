using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.ViewModels;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Factories;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the payments made by one or more flats of a specified <see cref="Block"/> during a specified <see cref="Month"/> and Year.
    /// </summary>
    public class PaymentsReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public PaymentsReportViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService)
            : base(summaryViewModel, navigationService)
        {

        }

        #endregion

        #region Public Properties

        public IReportOptions ReportOptions { get => Get<IReportOptions>(); set => Set(value); }

        public PaymentsReportType PaymentsReportType { get => Get<PaymentsReportType>(); set { Set(value); UpdateReport(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            PaymentsReportType = PaymentsReportType.Monthwise;
        }

        private void UpdateReport()
        {
            ReportOptions = ReportOptionsFactory.Create(PaymentsReportType);

            ReportOptions.ReportOptionChanged -= OnReportOptionChanged;
            ReportOptions.ReportOptionChanged += OnReportOptionChanged;

            OnReportOptionChanged();
        }

        private void OnReportOptionChanged()
        {
            Report = PaymentsReportFactory.Create(PaymentsReportType, Block, ReportOptions);
            Report.GenerateContents();
        }

        #endregion

    }
}

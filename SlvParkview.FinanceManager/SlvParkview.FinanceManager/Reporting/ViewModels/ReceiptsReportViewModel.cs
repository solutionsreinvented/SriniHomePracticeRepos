using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.ViewModels;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Factories;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the receipts made by one or more flats of a specified <see cref="Block"/> during a specified <see cref="Month"/> and Year.
    /// </summary>
    public class ReceiptsReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public ReceiptsReportViewModel(SummaryViewModel summaryViewModel)
            : base(summaryViewModel)
        {

        }

        #endregion

        #region Public Properties

        public IReportOptions ReportOptions { get => Get<IReportOptions>(); set => Set(value); }

        public ReceiptsReportType ReceiptsReportType { get => Get<ReceiptsReportType>(); set { Set(value); UpdateReport(); } }

        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();

            ReceiptsReportType = ReceiptsReportType.Monthwise;
        }

        private void UpdateReport()
        {
            ReportOptions = ReportOptionsFactory.Create(ReceiptsReportType);

            ReportOptions.ReportOptionChanged -= OnReportOptionChanged;
            ReportOptions.ReportOptionChanged += OnReportOptionChanged;

            OnReportOptionChanged();
        }

        private void OnReportOptionChanged()
        {
            Report = ReceiptsReportFactory.Create(ReceiptsReportType, Block, ReportOptions);
            Report.GenerateContents();
        }

        #endregion

    }
}

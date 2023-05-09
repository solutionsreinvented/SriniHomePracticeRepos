using ReInvented.Shared.Commands;

using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.ViewModels;

using System;
using System.Windows.Input;

namespace SlvParkview.FinanceManager.Reporting.ViewModels
{
    /// <summary>
    /// Reports the outstandings of each flat on a specified date for a specified block.
    /// </summary>
    public class BlockOutstandingsReportViewModel : ReportViewModelBase
    {
        #region Parameterized Constructor

        public BlockOutstandingsReportViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService) : base(summaryViewModel, navigationService)
        {

        }

        #endregion

        #region Public Properties

        public DateTime ReportTill { get => Get<DateTime>(); set { Set(value); UpdateReport(); } }

        public OutstandingsFilter Filter { get => Get(OutstandingsFilter.All); set { Set(value); UpdateReport(); } }

        public bool ShowOnlyPenalties { get => Get<bool>(); set { Set(value); Filter = OutstandingsFilter.All; UpdateReport(); } }

        public string OutstandingHeader { get => Get<string>(); private set => Set(value); }

        public Flat SelectedFlat { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Commands
        public ICommand AddPaymentCommand { get; set; }
        #endregion

        #region Command Handlers
        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(_summaryViewModel, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = paymentViewModel;
        }
        #endregion

        #region Private Helpers

        private protected override void Initialize()
        {
            base.Initialize();
            ReportTill = DateTime.Today;

            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
        }

        private void UpdateReport()
        {
            OutstandingHeader = $"Outstanding as on {ReportTill:dd-MMM-yyyy}";
            Report = new BlockOutstandingsReport(Block, ReportTill, Filter, ShowOnlyPenalties);
            Report.GenerateContents();
        }

        #endregion
    }
}

using ReInvented.Shared.Commands;
using System.Collections.Generic;
using System.Windows.Input;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Reporting.ViewModels;
using SlvParkview.FinanceManager.Factories;

namespace SlvParkview.FinanceManager.ViewModels
{
    /// <summary>
    /// ViewModel which handles the generation of different sorts of reports.
    /// </summary>
    public class ReportingViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;

        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ReportingViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ReportingViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService)
            : this()
        {
            _summaryViewModel = summaryViewModel;
            _navigationService = navigationService;

            ReportType = ReportType.FlatTransactionsHistory;
        }

        #endregion

        #region Public Properties

        public ReportType ReportType { get => Get<ReportType>(); set { Set(value); UpdateReportViewModel(); } }

        public ReportViewModelBase CurrentReportViewModel { get => Get<ReportViewModelBase>(); set => Set(value); }

        #endregion

        #region Read-only Properties

        public List<TransactionInfo> TransactionsSummary
        {
            get => Get<List<TransactionInfo>>();
            private set => Set(value);
        }

        #endregion

        #region Public Commands

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand ShowReportCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _summaryViewModel;
        }
        private void OnGenerateReport()
        {
            CurrentReportViewModel.Report.SaveReport();
        }

        private void OnShowReport()
        {
            ReportViewerViewModel reportViewerViewModel = new ReportViewerViewModel(_summaryViewModel, _navigationService, CurrentReportViewModel.Report);
            _navigationService.CurrentViewModel = reportViewerViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
            ShowReportCommand = new RelayCommand(OnShowReport, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void UpdateReportViewModel()
        {
            CurrentReportViewModel = ReportViewModelFactory.Create(_summaryViewModel, _navigationService, ReportType);
        }

        #endregion

    }
}


using ReInvented.Shared.Commands;

using System;
using System.Collections.Generic;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.Enums;
using SlvParkview.FinanceManager.Reporting.Interfaces;
using SlvParkview.FinanceManager.Reporting.Models;
using SlvParkview.FinanceManager.Reporting.ViewModels;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportingViewModel : BaseViewModel
    {
        #region Private Fields

        ///private readonly string _reportTargetDirectory;

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

        public ICommand GenerateCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            GenerateCommand = new RelayCommand(OnGenerate, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        #endregion

        private void UpdateReportViewModel()
        {
            if (ReportType == ReportType.BlockOutstandings)
            {
                CurrentReportViewModel = new BlockOutstandingsReportViewModel(_summaryViewModel);
            }
            else if (ReportType == ReportType.PaymentsInAMonth)
            {
                CurrentReportViewModel = new MonthwisePaymentsReportViewModel(_summaryViewModel);
            }
            else if (ReportType == ReportType.FlatTransactionsHistory)
            {
                CurrentReportViewModel = new FlatTransactionsHistoryReportViewModel(_summaryViewModel);
            }
        }


        private void OnGenerate()
        {
            CurrentReportViewModel.Report.SaveReport();
        }
    }
}


using ReInvented.Shared.Commands;

using System;
using System.Collections.Generic;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System.IO;
using ReInvented.DataAccess;
using ReInvented.DataAccess.Interfaces;
using System.Windows;
using SlvParkview.FinanceManager.Reporting;
using SlvParkview.FinanceManager.Enums;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Fields

        private string _reportTargetDirectory;

        private readonly SummaryViewModel _summaryViewModel;

        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ReportViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ReportViewModel(SummaryViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
            : this()
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public ReportType ReportType { get => Get(ReportType.BlockOutstandings); set => Set(value); }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        public FlatTransactionsHistoryReport FlatTransactionsReport { get => Get<FlatTransactionsHistoryReport>(); set => Set(value); }

        public BlockOustandingsReport OverviewReport { get => Get<BlockOustandingsReport>(); set => Set(value); }

        public IReport Report { get => Get<IReport>(); set => Set(value); }

        public DateTime ReportTill { get => Get(DateTime.Today); set => Set(value); }

        public bool IncludeFlatInformation { get => Get(true); set => Set(value); }

        public bool ShowTransactionHistory { get => Get(true); set => Set(value); }

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

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_summaryViewModel, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(_summaryViewModel, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_summaryViewModel, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);
            GenerateCommand = new RelayCommand(OnGenerate, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        #endregion

        private void OnGenerate()
        {
            if (ReportType == ReportType.BlockOutstandings)
            {
                Report = new BlockOustandingsReport(_summaryViewModel.Block, ReportTill);
            }
            else if(ReportType == ReportType.PaymentsInAMonth)
            {
                Report = new MonthwisePaymentsReport(_summaryViewModel.Block, (Month)ReportTill.Month, ReportTill.Year);
            }
            else if (ReportType == ReportType.FlatTransactionsHistory)
            {
                Report = new FlatTransactionsHistoryReport(FlatToBeProcessed, ReportTill);
            }

            Report.Create();

        }
    }
}

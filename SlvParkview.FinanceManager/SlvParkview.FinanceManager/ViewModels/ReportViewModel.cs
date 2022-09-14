
using ReInvented.Shared.Commands;

using System;
using System.Collections.Generic;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _sender;
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
            _sender = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        public List<FlatTransactionRecord> TransactionsSummary => FlatToBeProcessed.TransactionsSummary;

        public DateTime ReportTill { get => Get(DateTime.Today); set => Set(value); }

        public bool IncludeFlatInformation { get => Get(true); set => Set(value); }

        public bool ShowTransactionHistory { get => Get(true); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

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

        #region Command Handlers

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_sender, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerate()
        {
            throw new NotImplementedException();
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

    }
}

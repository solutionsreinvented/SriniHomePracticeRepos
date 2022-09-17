using ReInvented.Shared.Commands;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System;
using System.Windows.Input;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class EditFlatViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;

        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private EditFlatViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor
        public EditFlatViewModel(SummaryViewModel summaryViewModel, NavigationService navigationService, Flat selectedFlat)
            : this()
        {
            _summaryViewModel = summaryViewModel;
            _navigationService = navigationService;

            FlatToBeProcessed = selectedFlat;
        }
        #endregion

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        public Payment SelectedPayment { get => Get<Payment>(); set => Set(value); }

        public Expense SelectedExpense { get => Get<Expense>(); set => Set(value); }


        #region Public Commands

        public ICommand SaveFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveFlat()
        {

        }

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

        private void OnGenerateReports()
        {
            ReportViewModel reportViewModel = new ReportViewModel(_summaryViewModel, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = reportViewModel;
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        #endregion
    }
}

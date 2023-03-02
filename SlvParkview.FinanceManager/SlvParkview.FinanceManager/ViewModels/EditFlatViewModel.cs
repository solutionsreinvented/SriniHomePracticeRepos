using ReInvented.Shared.Commands;

using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

using System.Collections.Generic;
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

            TargetFlat = selectedFlat;
        }
        #endregion

        #region Public Properties

        public Flat TargetFlat { get => Get<Flat>(); set => Set(value); }

        public Payment SelectedPayment { get => Get<Payment>(); set { Set(value); RaisePropertyChanged(nameof(CanModifyPayment)); } }

        public Expense SelectedExpense { get => Get<Expense>(); set { Set(value); RaisePropertyChanged(nameof(CanModifyExpense)); } }

        #endregion

        #region Read-only Properties

        public IEnumerable<Flat> Flats => _summaryViewModel.Block.Flats;

        public DataManagementService DataManagementService { get => Get<DataManagementService>(); private set => Set(value); }

        public bool CanModifyPayment => SelectedPayment != null;

        public bool CanModifyExpense => SelectedExpense != null;

        #endregion

        #region Public Commands

        public ICommand SaveFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand DeletePaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand DeleteExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SavePaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SaveExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveFlat()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_summaryViewModel, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerateReports()
        {
            ReportingViewModel reportViewModel = new ReportingViewModel(_summaryViewModel, _navigationService);
            _navigationService.CurrentViewModel = reportViewModel;
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _summaryViewModel;
        }

        private void OnDeletePayment()
        {
            if (SelectedPayment != null)
            {
                _ = TargetFlat.Payments.Remove(SelectedPayment);
            }
        }

        private void OnDeleteExpense()
        {
            if (SelectedExpense != null)
            {
                _ = TargetFlat.Expenses.Remove(SelectedExpense);
            }
        }

        private void OnSavePayment()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        private void OnSaveExpense()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            DataManagementService = DataManagementService.Instance;

            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);

            DeletePaymentCommand = new RelayCommand(OnDeletePayment, true);
            DeleteExpenseCommand = new RelayCommand(OnDeleteExpense, true);

            SavePaymentCommand = new RelayCommand(OnSavePayment, true);
            SaveExpenseCommand = new RelayCommand(OnSaveExpense, true);
        }

        #endregion
    }
}

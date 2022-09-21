using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ExpenseViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ExpenseViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ExpenseViewModel(SummaryViewModel sender, NavigationService navigationService, Flat targetFlat)
            : this()
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            TargetFlat = targetFlat;
        }

        #endregion

        #region Public Properties

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public Flat TargetFlat { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveExpense()
        {
            Flat targetFlat = _summaryViewModel.Block.Flats.FirstOrDefault(f => f.Description == TargetFlat.Description);

            if (!targetFlat.ContainsSimilar(Expense))
            {
                AddExpenseTo(targetFlat);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("It appears that the entry already exists. Do you still want to add this?", "Duplicate entry identified", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    AddExpenseTo(targetFlat);
                }
            }
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

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Expense = new Expense();
            SaveExpenseCommand = new RelayCommand(OnSaveExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void AddExpenseTo(Flat flat)
        {
            flat.AddExpense(Expense);

            _ = MessageBox.Show("Expense added successfully!", "Entry successful", MessageBoxButton.OK);

            Expense = new Expense();
        }

        #endregion

    }
}

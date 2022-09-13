using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class ExpenseViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ExpenseViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ExpenseViewModel(SummaryViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
            : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public Expense Expense { get => Get<Expense>(); set => Set(value); }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveExpense()
        {
            Flat targetFlat = _sender.ApartmentBlock.Flats.FirstOrDefault(f => f.Description == FlatToBeProcessed.Description);

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
            PaymentViewModel paymentViewModel = new PaymentViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_sender, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerateReport()
        {

        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Expense = new Expense();
            SaveExpenseCommand = new RelayCommand(OnSaveExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
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

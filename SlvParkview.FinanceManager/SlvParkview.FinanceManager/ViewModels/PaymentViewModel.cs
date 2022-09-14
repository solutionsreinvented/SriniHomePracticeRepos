using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.Models;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private PaymentViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public PaymentViewModel(SummaryViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
            : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            FlatToBeProcessed = flatToBeProcessed;
        }

        #endregion

        #region Public Properties

        public Payment Payment { get => Get<Payment>(); set => Set(value); }

        public Flat FlatToBeProcessed { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SavePaymentCommand { get; set; }

        public ICommand AddExpenseCommand { get; set; }

        public ICommand AddCommonExpenseCommand { get; set; }

        public ICommand GenerateReportsCommand { get; set; }

        public ICommand GoToSummaryCommand { get; set; }

        #endregion

        #region Command Handlers

        private void OnSavePayment()
        {
            Flat targetFlat = _sender.Block.Flats.FirstOrDefault(f => f.Description == FlatToBeProcessed.Description);

            if (!targetFlat.ContainsSimilar(Payment))
            {
                AddPaymentTo(targetFlat);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("It appears that the entry already exists. Do you still want to add this?", "Duplicate entry identified", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    AddPaymentTo(targetFlat);
                }
            }
        }

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(_sender, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerateReports()
        {
            ReportViewModel reportViewModel = new ReportViewModel(_sender, _navigationService, FlatToBeProcessed);
            _navigationService.CurrentViewModel = reportViewModel;
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Helper Methods

        private void Initialize()
        {
            Payment = new Payment();
            SavePaymentCommand = new RelayCommand(OnSavePayment, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void AddPaymentTo(Flat flat)
        {
            flat.AddPayment(Payment);

            _ = MessageBox.Show("Payment added successfully!", "Entry successful", MessageBoxButton.OK);

            Payment = Payment.Clone(Payment);
        }

        #endregion

    }

}

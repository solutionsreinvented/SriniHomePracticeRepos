using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.Models;
using System;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private PaymentViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public PaymentViewModel(SummaryViewModel sender, NavigationService navigationService, Flat targetFlat)
            : this()
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            TargetFlat = targetFlat;
        }

        #endregion

        #region Public Properties

        public Payment Payment { get => Get<Payment>(); set => Set(value); }

        public Flat TargetFlat { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SavePaymentCommand { get; set; }

        public ICommand SaveFlatCommand { get; set; }

        public ICommand AddExpenseCommand { get; set; }

        public ICommand AddCommonExpenseCommand { get; set; }

        public ICommand GenerateReportsCommand { get; set; }

        public ICommand GoToSummaryCommand { get; set; }

        #endregion

        #region Command Handlers

        private void OnSavePayment()
        {
            Flat targetFlat = _summaryViewModel.Block.Flats.FirstOrDefault(f => f.Description == TargetFlat.Description);

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
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = expenseViewModel;
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

        private void OnSaveFlat()
        {
            DataManagementService.Instance.SaveData(_summaryViewModel.Block);
        }

        #endregion

        #region Helper Methods

        private void Initialize()
        {
            Payment = new Payment() { RecordedOn = DateTime.Today};

            SavePaymentCommand = new RelayCommand(OnSavePayment, true);
            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void AddPaymentTo(Flat flat)
        {
            flat.AddPayment(Payment);
            _ = MessageBox.Show("Payment added successfully!", "Entry successful", MessageBoxButton.OK);

            DataManagementService.Instance.SaveData(_summaryViewModel.Block);
            Payment = new Payment();
        }

        #endregion

    }

}

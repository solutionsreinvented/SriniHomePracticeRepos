using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System;
using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class SummaryViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private SummaryViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public SummaryViewModel(NavigationService navigationService) : this()
        {
            _navigationService = navigationService;
            ApartmentBlock = BlockInitialDataProvider.Generate();
        }

        #endregion


        #region Public Properties

        public ApartmentBlock ApartmentBlock { get => Get<ApartmentBlock>(); set => Set(value); }

        public Flat SelectedFlat
        {
            get => Get<Flat>();
            set
            {
                CanProcessFlat = true;
                Set(value);
            }
        }

        public bool CanProcessFlat { get => Get<bool>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnAddExpense()
        {
            ExpenseViewModel addExpenseViewModel = new ExpenseViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = addExpenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel addPaymentViewModel = new PaymentViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = addPaymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(this, _navigationService, ApartmentBlock);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerateReport()
        {
            ReportViewModel reportViewModel = new ReportViewModel(_navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = reportViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            CanProcessFlat = false;

            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);
            GenerateReportCommand = new RelayCommand(OnGenerateReport, true);
        }

        #endregion
    }
}

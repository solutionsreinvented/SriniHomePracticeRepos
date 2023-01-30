using ReInvented.Shared.Commands;

using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
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
            ///ApartmentBlock = BlockInitialDataProvider.Generate();
        }

        #endregion

        #region Public Properties


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

        public ICommand EditFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnEditFlat()
        {
            EditFlatViewModel editFlatViewModel = new EditFlatViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = editFlatViewModel;
        }

        private void OnAddExpense()
        {
            ExpenseViewModel expenseViewModel = new ExpenseViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = expenseViewModel;
        }

        private void OnAddPayment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = paymentViewModel;
        }

        private void OnAddCommonExpense()
        {
            CommonExpenseViewModel commonExpenseViewModel = new CommonExpenseViewModel(this, _navigationService);
            _navigationService.CurrentViewModel = commonExpenseViewModel;
        }

        private void OnGenerateReports()
        {
            ReportingViewModel reportViewModel = new ReportingViewModel(this, _navigationService);
            _navigationService.CurrentViewModel = reportViewModel;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            CanProcessFlat = false;

            EditFlatCommand = new RelayCommand(OnEditFlat, true);
            AddExpenseCommand = new RelayCommand(OnAddExpense, true);
            AddPaymentCommand = new RelayCommand(OnAddPayment, true);
            AddCommonExpenseCommand = new RelayCommand(OnAddCommonExpense, true);
            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
        }

        #endregion
    }
}

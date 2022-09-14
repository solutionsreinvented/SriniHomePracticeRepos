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

        public Block Block { get => Get<Block>(); set { Set(value); ShowContent = value != null; } }

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
        public bool ShowContent { get => Get<bool>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand AddPaymentCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonExpenseCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

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
            ReportViewModel reportViewModel = new ReportViewModel(this, _navigationService, SelectedFlat);
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
            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
        }

        #endregion
    }
}

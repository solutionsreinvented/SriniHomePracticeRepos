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

        public ICommand EditFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddReceiptCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnEditFlat()
        {
            EditFlatViewModel editFlatViewModel = new EditFlatViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = editFlatViewModel;
        }

        private void OnAddBill()
        {
            BillViewModel billViewModel = new BillViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = billViewModel;
        }

        private void OnAddReceipt()
        {
            ReceiptViewModel receiptViewModel = new ReceiptViewModel(this, _navigationService, SelectedFlat);
            _navigationService.CurrentViewModel = receiptViewModel;
        }

        private void OnAddCommonBill()
        {
            CommonBillViewModel commonBillViewModel = new CommonBillViewModel(this, _navigationService);
            _navigationService.CurrentViewModel = commonBillViewModel;
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
            AddBillCommand = new RelayCommand(OnAddBill, true);
            AddReceiptCommand = new RelayCommand(OnAddReceipt, true);
            AddCommonBillCommand = new RelayCommand(OnAddCommonBill, true);
            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
        }

        #endregion
    }
}

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

        public Receipt SelectedReceipt { get => Get<Receipt>(); set { Set(value); RaisePropertyChanged(nameof(CanModifyReceipt)); } }

        public Bill SelectedBill { get => Get<Bill>(); set { Set(value); RaisePropertyChanged(nameof(CanModifyBill)); } }

        #endregion

        #region Read-only Properties

        public IEnumerable<Flat> Flats => _summaryViewModel.Block.Flats;

        public DataManagementService DataManagementService { get => Get<DataManagementService>(); private set => Set(value); }

        public bool CanModifyReceipt => SelectedReceipt != null;

        public bool CanModifyBill => SelectedBill != null;

        #endregion

        #region Public Commands

        public ICommand SaveFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddReceiptCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand DeleteReceiptCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand DeleteBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SaveReceiptCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SaveBillCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveFlat()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        private void OnAddBill()
        {
            BillViewModel billViewModel = new BillViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = billViewModel;
        }

        private void OnAddReceipt()
        {
            ReceiptViewModel receiptViewModel = new ReceiptViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = receiptViewModel;
        }

        private void OnAddCommonBill()
        {
            CommonBillViewModel commonBillViewModel = new CommonBillViewModel(_summaryViewModel, _navigationService);
            _navigationService.CurrentViewModel = commonBillViewModel;
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

        private void OnDeleteReceipt()
        {
            if (SelectedReceipt != null)
            {
                _ = TargetFlat.Receipts.Remove(SelectedReceipt);
            }
        }

        private void OnDeleteBill()
        {
            if (SelectedBill != null)
            {
                _ = TargetFlat.Bills.Remove(SelectedBill);
            }
        }

        private void OnSaveReceipt()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        private void OnSaveBill()
        {
            DataManagementService.SaveData(_summaryViewModel.Block);
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            DataManagementService = DataManagementService.Instance;

            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);
            AddBillCommand = new RelayCommand(OnAddBill, true);
            AddReceiptCommand = new RelayCommand(OnAddReceipt, true);
            AddCommonBillCommand = new RelayCommand(OnAddCommonBill, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);

            DeleteReceiptCommand = new RelayCommand(OnDeleteReceipt, true);
            DeleteBillCommand = new RelayCommand(OnDeleteBill, true);

            SaveReceiptCommand = new RelayCommand(OnSaveReceipt, true);
            SaveBillCommand = new RelayCommand(OnSaveBill, true);
        }

        #endregion
    }
}

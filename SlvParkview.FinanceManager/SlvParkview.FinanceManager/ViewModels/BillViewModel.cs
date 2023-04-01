using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;
using System.Collections.Generic;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class BillViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private BillViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public BillViewModel(SummaryViewModel sender, NavigationService navigationService, Flat targetFlat)
            : this()
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            TargetFlat = targetFlat;
        }

        #endregion

        #region Public Properties

        public Bill Bill { get => Get<Bill>(); set => Set(value); }

        public Flat TargetFlat { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Readonly Properties

        public IEnumerable<Flat> Flats => _summaryViewModel.Block.Flats;

        #endregion

        #region Public Commands

        public ICommand SaveBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SaveFlatCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddReceiptCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand AddCommonBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GenerateReportsCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveBill()
        {
            Flat targetFlat = _summaryViewModel.Block.Flats.FirstOrDefault(f => f.Description == TargetFlat.Description);

            if (!targetFlat.ContainsSimilar(Bill))
            {
                AddBillTo(targetFlat);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("It appears that the entry already exists. Do you still want to add this?", "Duplicate entry detected", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    AddBillTo(targetFlat);
                }
            }
        }

        private void OnSaveFlat()
        {
            DataManagementService.Instance.SaveData(_summaryViewModel.Block);
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

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Bill = new Bill();
            SaveBillCommand = new RelayCommand(OnSaveBill, true);
            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);

            AddReceiptCommand = new RelayCommand(OnAddReceipt, true);
            AddCommonBillCommand = new RelayCommand(OnAddCommonBill, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void AddBillTo(Flat flat)
        {
            flat.AddBill(Bill);
            DataManagementService.Instance.SaveData(_summaryViewModel.Block);

            _ = MessageBox.Show("Bill added successfully!", "Entry successful", MessageBoxButton.OK);

            Bill = new Bill();
        }

        #endregion

    }
}

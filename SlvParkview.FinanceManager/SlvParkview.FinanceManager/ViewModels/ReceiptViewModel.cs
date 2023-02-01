using ReInvented.Shared.Commands;

using System.Linq;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Services;
using SlvParkview.FinanceManager.Models;
using System;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class ReceiptViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly SummaryViewModel _summaryViewModel;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private ReceiptViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public ReceiptViewModel(SummaryViewModel sender, NavigationService navigationService, Flat targetFlat)
            : this()
        {
            _summaryViewModel = sender;
            _navigationService = navigationService;

            TargetFlat = targetFlat;
        }

        #endregion

        #region Public Properties

        public Receipt Receipt { get => Get<Receipt>(); set => Set(value); }

        public Flat TargetFlat { get => Get<Flat>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveReceiptCommand { get; set; }

        public ICommand SaveFlatCommand { get; set; }

        public ICommand AddBillCommand { get; set; }

        public ICommand AddCommonBillCommand { get; set; }

        public ICommand GenerateReportsCommand { get; set; }

        public ICommand GoToSummaryCommand { get; set; }

        #endregion

        #region Command Handlers

        private void OnSaveReceipt()
        {
            Flat targetFlat = _summaryViewModel.Block.Flats.FirstOrDefault(f => f.Description == TargetFlat.Description);

            if (!targetFlat.ContainsSimilar(Receipt))
            {
                AddReceiptTo(targetFlat);

            }
            else
            {
                MessageBoxResult result = MessageBox.Show("It appears that the entry already exists. Do you still want to add this?", "Duplicate entry identified", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    AddReceiptTo(targetFlat);
                }
            }
        }

        private void OnAddBill()
        {
            BillViewModel expenseViewModel = new BillViewModel(_summaryViewModel, _navigationService, TargetFlat);
            _navigationService.CurrentViewModel = expenseViewModel;
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

        private void OnSaveFlat()
        {
            DataManagementService.Instance.SaveData(_summaryViewModel.Block);
        }

        #endregion

        #region Helper Methods

        private void Initialize()
        {
            Receipt = new Receipt() { RecordedOn = DateTime.Today};

            SaveReceiptCommand = new RelayCommand(OnSaveReceipt, true);
            SaveFlatCommand = new RelayCommand(OnSaveFlat, true);
            AddBillCommand = new RelayCommand(OnAddBill, true);
            AddCommonBillCommand = new RelayCommand(OnAddCommonBill, true);

            GenerateReportsCommand = new RelayCommand(OnGenerateReports, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        private void AddReceiptTo(Flat flat)
        {
            flat.AddReceipt(Receipt);
            _ = MessageBox.Show("Receipt added successfully!", "Entry successful", MessageBoxButton.OK);

            DataManagementService.Instance.SaveData(_summaryViewModel.Block);
            Receipt = new Receipt();
        }

        #endregion

    }

}

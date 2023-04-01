using ReInvented.Shared.Commands;
using System.Windows;
using System.Windows.Input;
using SlvParkview.FinanceManager.Models;
using SlvParkview.FinanceManager.Services;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class CommonBillViewModel : BaseViewModel
    {

        #region Private Fields

        private readonly SummaryViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private CommonBillViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public CommonBillViewModel(SummaryViewModel sender, NavigationService navigationService) : this()
        {
            _sender = sender;
            _navigationService = navigationService;

            ApartmentBlockToBeProcessed = _sender.Block;
        }

        #endregion

        #region Public Properties

        public bool AllowDuplicateEntry { get => Get<bool>(); set => Set(value); }

        public Bill Bill { get => Get<Bill>(); set => Set(value); }

        public Block ApartmentBlockToBeProcessed { get => Get<Block>(); set => Set(value); }

        #endregion

        #region Public Commands

        public ICommand SaveBillCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand GoToSummaryCommand { get => Get<ICommand>(); set => Set(value); }

        #endregion

        #region Command Handlers

        private void OnSaveBill()
        {
            if (AllowDuplicateEntry)
            {
                _sender.Block.Flats.ForEach(f => f.AddBill(Bill));
                DataManagementService.Instance.SaveData(ApartmentBlockToBeProcessed);
            }
            else
            {
                foreach (Flat flat in _sender.Block.Flats)
                {
                    if (!flat.ContainsSimilar(Bill))
                    {
                        flat.AddBill(Bill);
                    }
                    else
                    {
                        MessageBoxResult result = MessageBox.Show($"It appears that the entry already exists for {flat.Description}. Do you still want to add this?", "Duplicate entry identified", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            flat.AddBill(Bill);
                        }
                    }
                }
            }

            Bill = new Bill();
        }

        private void OnGoToSummary()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Private Helpers

        private void Initialize()
        {
            Bill = new Bill();

            SaveBillCommand = new RelayCommand(OnSaveBill, true);
            GoToSummaryCommand = new RelayCommand(OnGoToSummary, true);
        }

        #endregion

    }
}
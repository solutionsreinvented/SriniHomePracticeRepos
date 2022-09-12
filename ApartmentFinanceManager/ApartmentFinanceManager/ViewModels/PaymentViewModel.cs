using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Commands;

using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        #region Private Fields

        private readonly BaseViewModel _sender;
        private readonly NavigationService _navigationService;

        #endregion

        #region Default Constructor

        private PaymentViewModel()
        {
            Initialize();
        }

        #endregion

        #region Parameterized Constructor

        public PaymentViewModel(BaseViewModel sender, NavigationService navigationService, Flat flatToBeProcessed)
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

        public ICommand CancelCommand { get; set; }

        #endregion

        #region Command Handlers

        private void OnCancel()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        private void OnSavePayment()
        {
            _navigationService.CurrentViewModel = _sender;
        }

        #endregion

        #region Helper Methods

        private void Initialize()
        {
            Payment = new Payment();
            SavePaymentCommand = new RelayCommand(OnSavePayment, true);
            CancelCommand = new RelayCommand(OnCancel, true);
        }

        #endregion

    }

}

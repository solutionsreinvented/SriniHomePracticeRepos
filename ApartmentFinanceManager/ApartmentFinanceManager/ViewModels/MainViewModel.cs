using ApartmentFinanceManager.Models;
using ApartmentFinanceManager.Services;

using ReInvented.Shared.Store;

namespace ApartmentFinanceManager.ViewModels
{
    public class MainViewModel : PropertyStore
    {

        public MainViewModel()
        {
            CurrentViewModel = new AddPaymentViewModel();
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }

    }
}

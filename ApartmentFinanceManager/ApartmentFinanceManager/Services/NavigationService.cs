using ApartmentFinanceManager.ViewModels;

using ReInvented.Shared.Stores;

using System;

namespace ApartmentFinanceManager.Services
{
    public class NavigationService : PropertyStore
    {

        public event Action CurrentViewModelChanged;

        public NavigationService()
        {

        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set { Set(value); CurrentViewModelChanged.Invoke(); } }

    }
}


using ReInvented.Shared.Stores;

using System;
using SlvParkview.FinanceManager.ViewModels;

namespace SlvParkview.FinanceManager.Services
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

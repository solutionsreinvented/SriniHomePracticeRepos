using System;

using ReIn.Navigation.ViewModels;

namespace ReIn.Navigation.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;

        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                RaiseCurrentViewModelChanged();
            }
        }

        private void RaiseCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}

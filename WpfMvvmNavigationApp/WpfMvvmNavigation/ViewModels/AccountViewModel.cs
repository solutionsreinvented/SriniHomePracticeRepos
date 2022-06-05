using System;
using System.Windows.Input;

using WpfMvvmNavigation.Commands;
using WpfMvvmNavigation.Stores;

namespace WpfMvvmNavigation.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public AccountViewModel(NavigationStore navigationStore)
        {
            Initialize(navigationStore);
        }

        private void Initialize(NavigationStore navigationStore)
        {
            Title = "Account View Model...";
            _navigationStore = navigationStore;
            NavigateToHomeCommand = new RelayCommand(NavigateToHome, true);
        }

        private void NavigateToHome()
        {
            _navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
        }

        public ICommand NavigateToHomeCommand { get; set; }

    }
}

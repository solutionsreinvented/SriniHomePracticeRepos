using System;
using System.Windows.Input;

using WpfMvvmNavigation.Commands;
using WpfMvvmNavigation.Stores;

namespace WpfMvvmNavigation.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public HomeViewModel(NavigationStore navigationStore)
        {
            Initialize(navigationStore);
        }

        private void Initialize(NavigationStore navigationStore)
        {
            Title = "Home View Model...";
            _navigationStore = navigationStore;
            NavigateToAccountCommand = new RelayCommand(NavigateToAccount, true);
        }

        private void NavigateToAccount()
        {
            _navigationStore.CurrentViewModel = new AccountViewModel(_navigationStore);
        }

        public ICommand NavigateToAccountCommand { get; set; }
    }
}

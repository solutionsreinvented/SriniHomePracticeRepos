using System.Windows.Input;

using ReIn.Navigation.Commands;
using ReIn.Navigation.Services;
using ReIn.Navigation.Stores;

namespace ReIn.Navigation.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateAccountCommand = new NavigateCommand<LoginViewModel>
                (new NavigationService<LoginViewModel>(navigationStore, () => new LoginViewModel(navigationStore)));
        }

        public string WelcomeMessage => "Welcome to my application.";

        public ICommand NavigateAccountCommand { get; }
    }
}

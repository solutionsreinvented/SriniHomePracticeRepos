using System.Windows;

using ReIn.Navigation.Stores;
using ReIn.Navigation.ViewModels;

namespace ReIn.Navigation.Commands
{
    public class LoginCommand : BaseCommand
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly NavigationStore _navigationStore;

        public LoginCommand(LoginViewModel loginViewModel, NavigationStore navigationStore)
        {
            _loginViewModel = loginViewModel;
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _ = MessageBox.Show($"Logging in {_loginViewModel.Username}...");

            _navigationStore.CurrentViewModel = new AccountViewModel(_navigationStore);
            /// Navigate to the account page.
        }
    }
}

using System.Windows.Input;

using ReIn.Navigation.Commands;
using ReIn.Navigation.Stores;

namespace ReIn.Navigation.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private string _password;

        //private readonly NavigationStore _navigationStore;

        public LoginViewModel(NavigationStore navigationStore)
        {
            //_navigationStore = navigationStore;
            LoginCommand = new LoginCommand(this, navigationStore);
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

    }
}

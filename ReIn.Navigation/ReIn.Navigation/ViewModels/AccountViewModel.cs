using System.Windows.Input;

using ReIn.Navigation.Commands;
using ReIn.Navigation.Services;
using ReIn.Navigation.Stores;

namespace ReIn.Navigation.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {

        public AccountViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore)));
        }

        public string Name => "Srinivasa Rao M";

        public ICommand NavigateHomeCommand { get; }
    }
}

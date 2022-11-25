
using ReIn.ViewModelPercolation.Stores;

using ReInvented.Shared.Stores;

namespace ReIn.ViewModelPercolation.ViewModels
{
    public class BaseViewModel : PropertyStore
    {
        private protected readonly NavigationStore _navigationStore;

        public BaseViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}

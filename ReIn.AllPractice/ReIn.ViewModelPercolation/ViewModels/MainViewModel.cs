using ReIn.ViewModelPercolation.Stores;

using ReInvented.Shared.Stores;

namespace ReIn.ViewModelPercolation.ViewModels
{
    public class MainViewModel : PropertyStore
    {

        private readonly NavigationStore _navigationStore;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }



    }
}

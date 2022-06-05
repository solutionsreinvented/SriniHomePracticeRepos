using System.ComponentModel;

using WpfMvvmNavigation.Stores;

namespace WpfMvvmNavigation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationStore _navigationStore;

        public ViewModelBase CurrentViewModel { get; set; }

        public MainViewModel()
        {
            _navigationStore = new NavigationStore();
            _navigationStore.ViewModelChanged += OnViewModelChanged;
            CurrentViewModel = new HomeViewModel(_navigationStore);
        }

        private void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }
    }
}

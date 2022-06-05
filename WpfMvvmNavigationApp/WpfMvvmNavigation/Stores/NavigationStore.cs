using System.ComponentModel;

using WpfMvvmNavigation.Models;
using WpfMvvmNavigation.ViewModels;

namespace WpfMvvmNavigation.Stores
{
    public class NavigationStore : NotifyPropertyChanged
    {
        private ViewModelBase _currentViewModel;

        public event PropertyChangedEventHandler ViewModelChanged;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                ViewModelChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
            }
        }

        public NavigationStore()
        {

        }
    }
}

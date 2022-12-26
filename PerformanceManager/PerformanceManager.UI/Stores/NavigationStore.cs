using System.ComponentModel;

using PerformanceManager.UI.ViewModels;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationStore(IDialogService dialogService)
        {
            ///CurrentViewModel = new ChangePasswordViewModel(this);
            DialogService = dialogService;
        }

        public IDialogService DialogService { get; }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
                }
            }
        }

    }
}

using System.ComponentModel;

using PerformanceManager.UI.ViewModels;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.Stores
{
    public class NavigationStore
    {
        private ViewModelBase _currentViewModel;
        private readonly IDialogService _dialogService;

        public event PropertyChangedEventHandler PropertyChanged;

        public NavigationStore(IDialogService dialogService)
        {
            ///CurrentViewModel = new ChangePasswordViewModel(this);
            _dialogService = dialogService;
        }

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

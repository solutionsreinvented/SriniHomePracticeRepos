using System.ComponentModel;

using ActivityTracker.UI.Base;

using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.Stores
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

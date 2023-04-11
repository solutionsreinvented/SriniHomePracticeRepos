
using ActivityTracker.Domain.Stores;
using ActivityTracker.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace ActivityTracker.UI.ViewModels
{
    public abstract class ViewModelBase : PropertyStore
    {
        private protected readonly NavigationStore _navigationStore;
        private protected readonly IDialogService _dialogService;

        public ViewModelBase(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _dialogService = _navigationStore.DialogService;
        }
    }
}

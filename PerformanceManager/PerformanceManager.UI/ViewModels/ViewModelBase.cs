
using PerformanceManager.Domain.Stores;
using PerformanceManager.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public abstract class ViewModelBase : PropertyStore
    {
        private protected readonly NavigationStore _navigationStore;
        private protected readonly IDialogService _dialogService;

        public ViewModelBase(NavigationStore navigationStore, IDialogService dialogService)
        {
            _navigationStore = navigationStore;
            _dialogService = dialogService;
        }
    }
}

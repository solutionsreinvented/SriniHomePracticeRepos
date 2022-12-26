using PerformanceManager.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace PerformanceManager.UI.ViewModels
{
    public class ActivityConflictViewModel : ViewModelBase
    {
        public ActivityConflictViewModel(NavigationStore navigationStore, IDialogService dialogService) 
            : base(navigationStore, dialogService)
        {

        }
    }
}

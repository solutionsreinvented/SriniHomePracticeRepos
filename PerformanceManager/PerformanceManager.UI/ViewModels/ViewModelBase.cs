
using PerformanceManager.Domain.Stores;
using PerformanceManager.UI.Stores;

namespace PerformanceManager.UI.ViewModels
{
    public abstract class ViewModelBase : PropertyStore
    {
        private protected readonly NavigationStore _navigationStore;

        public ViewModelBase(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
    }
}

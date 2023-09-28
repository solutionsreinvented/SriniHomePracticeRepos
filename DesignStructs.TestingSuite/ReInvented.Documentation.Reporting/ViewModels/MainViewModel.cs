using ReInvented.Shared.Stores;

namespace ReInvented.Documentation.Reporting.ViewModels
{
    public class MainViewModel : ErrorsEnabledPropertyStore
    {
        public MainViewModel()
        {
            CurrentViewModel = new ProjectViewModel();
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }
    }
}

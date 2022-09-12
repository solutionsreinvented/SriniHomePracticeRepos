using ApartmentFinanceManager.Services;

using ReInvented.Shared.Stores;

using System.Windows.Input;

namespace ApartmentFinanceManager.ViewModels
{
    public class MainViewModel : PropertyStore
    {

        private NavigationService _navigationService;

        public MainViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            _navigationService = new NavigationService();

            _navigationService.CurrentViewModelChanged -= OnNavigationServiceCurrentViewModelChanged;
            _navigationService.CurrentViewModelChanged += OnNavigationServiceCurrentViewModelChanged;

            BaseViewModel summaryViewModel = new SummaryViewModel(_navigationService);

            _navigationService.CurrentViewModel = summaryViewModel;
        }

        private void OnNavigationServiceCurrentViewModelChanged()
        {
            CurrentViewModel = _navigationService.CurrentViewModel;
            ///RaisePropertyChanged(nameof(CurrentViewModel));
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }

        public ICommand SaveDataCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand RetrieveDataCommand { get => Get<ICommand>(); set => Set(value); }

    }
}

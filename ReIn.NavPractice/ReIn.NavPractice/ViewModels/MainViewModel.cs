using ReIn.NavPractice.Services;

using ReInvented.Shared.Stores;

namespace ReIn.NavPractice.ViewModels
{
    public class MainViewModel : PropertyStore
    {

        private readonly SupportStructureViewModel _supportStructureViewModel;
        private readonly SectionSelectionViewModel _sectionSelectionViewModel;

        public MainViewModel()
        {
            NavigationService = new NavigationService();

            _supportStructureViewModel = new SupportStructureViewModel(NavigationService);
            _sectionSelectionViewModel = new SectionSelectionViewModel(NavigationService);

            NavigationService.SectionSelectionViewModel = _sectionSelectionViewModel;
            NavigationService.SupportStructureViewModel = _supportStructureViewModel;

            NavigationService.CurrentViewModel = _supportStructureViewModel;
        }

        public NavigationService NavigationService { get => Get<NavigationService>(); set => Set(value); }

    }
}

using ReIn.NavPractice.ViewModels;

using ReInvented.Shared.Stores;

namespace ReIn.NavPractice.Services
{
    public class NavigationService : PropertyStore
    {
        public NavigationService()
        {
            CurrentViewModel = SupportStructureViewModel;
        }

        public BaseViewModel CurrentViewModel { get => Get<BaseViewModel>(); set => Set(value); }

        public SupportStructureViewModel SupportStructureViewModel { get => Get<SupportStructureViewModel>(); set => Set(value); }

        public SectionSelectionViewModel SectionSelectionViewModel { get => Get<SectionSelectionViewModel>(); set => Set(value); }
    }
}

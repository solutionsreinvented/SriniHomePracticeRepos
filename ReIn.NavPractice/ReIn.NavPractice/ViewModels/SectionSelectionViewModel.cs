using System.Windows.Input;

using ReIn.NavPractice.Models;
using ReIn.NavPractice.Services;

using ReInvented.Shared.Commands;

namespace ReIn.NavPractice.ViewModels
{
    public class SectionSelectionViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;

        public SectionSelectionViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            SelectionCompletedCommand = new RelayCommand(OnSelectionCompleted, true);
        }

        public Section Section { get => Get<Section>(); set => Set(value); }

        public ICommand SelectionCompletedCommand { get => Get<ICommand>(); set => Set(value); }

        private void OnSelectionCompleted()
        {
            _navigationService.CurrentViewModel = _navigationService.SupportStructureViewModel;
        }
    }
}

using System.Windows.Input;

using ReIn.NavPractice.Models;
using ReIn.NavPractice.Services;

using ReInvented.Shared.Commands;

namespace ReIn.NavPractice.ViewModels
{
    public class SupportStructureViewModel : BaseViewModel
    {
        private readonly NavigationService _navigationService;

        public SupportStructureViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;

            SupportStructure = new SupportStructure();

            SelectColumnCommand = new RelayCommand(OnSelectColumn, true);
            SelectCrossBracingCommand = new RelayCommand(OnSelectCrossBracing, true);
            SelectRadialBracingCommand = new RelayCommand(OnSelectRadialBracing, true);
        }

        private void OnSelectRadialBracing()
        {
            _navigationService.SectionSelectionViewModel.Section = SelectedGridRow.RadialBracing;
            _navigationService.CurrentViewModel = _navigationService.SectionSelectionViewModel;
        }

        private void OnSelectCrossBracing()
        {
            _navigationService.SectionSelectionViewModel.Section = SelectedGridRow.CrossBracing;
            _navigationService.CurrentViewModel = _navigationService.SectionSelectionViewModel;
        }

        private void OnSelectColumn()
        {
            _navigationService.SectionSelectionViewModel.Section = SelectedGridRow.Column;
            _navigationService.CurrentViewModel = _navigationService.SectionSelectionViewModel;
        }

        public SupportStructure SupportStructure { get => Get<SupportStructure>(); set => Set(value); }

        public GridRow SelectedGridRow { get => Get<GridRow>(); set => Set(value); }

        public ICommand SelectColumnCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SelectCrossBracingCommand { get => Get<ICommand>(); set => Set(value); }

        public ICommand SelectRadialBracingCommand { get => Get<ICommand>(); set => Set(value); }

    }
}

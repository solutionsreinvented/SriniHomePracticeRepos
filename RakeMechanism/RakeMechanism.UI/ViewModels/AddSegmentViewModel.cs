using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RakeMechanism.UI.ViewModels
{
    public partial class AddSegmentViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _selectedSegmentType = "Cage Frame";

        [ObservableProperty]
        private IStructureSegmentViewModel _newSegment = new CageSegmentViewModel();

        [RelayCommand]
        private void SelectCage()
        {
            SelectedSegmentType = "Cage Frame";
            NewSegment = new CageSegmentViewModel();
        }

        [RelayCommand]
        private void SelectTube()
        {
            SelectedSegmentType = "Tube Mesh";
            NewSegment = new TubeSegmentViewModel();
        }
    }
}

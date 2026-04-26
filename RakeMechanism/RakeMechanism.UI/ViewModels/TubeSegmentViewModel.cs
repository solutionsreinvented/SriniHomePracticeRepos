using CommunityToolkit.Mvvm.ComponentModel;
using RakeMechanism.Geometry.Domain;

namespace RakeMechanism.UI.ViewModels
{
    public partial class TubeSegmentViewModel : ObservableObject, IStructureSegmentViewModel
    {
        public string SegmentType => "Tube Mesh";

        [ObservableProperty]
        private bool _hostsRakeArms;

        [ObservableProperty]
        private double _height = 10.0;

        [ObservableProperty]
        private double _computedStartElevation;

        [ObservableProperty]
        private double _startRadius = 5.0;

        [ObservableProperty]
        private double _endRadius = 5.0;

        [ObservableProperty]
        private int _verticalDivisions = 5;

        [ObservableProperty]
        private int _circumferentialDivisions = 36;

        public IStructureSegment ToDomainModel()
        {
            return new TubeSegment
            {
                HostsRakeArms = this.HostsRakeArms,
                StartElevation = this.ComputedStartElevation,
                EndElevation = this.ComputedStartElevation + this.Height,
                StartRadius = this.StartRadius,
                EndRadius = this.EndRadius,
                VerticalDivisions = this.VerticalDivisions,
                CircumferentialDivisions = this.CircumferentialDivisions
            };
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using RakeMechanism.Geometry.Domain;

namespace RakeMechanism.UI.ViewModels
{
    public partial class CageSegmentViewModel : ObservableObject, IStructureSegmentViewModel
    {
        public string SegmentType => "Cage Frame";

        [ObservableProperty]
        private bool _hostsRakeArms;

        [ObservableProperty]
        private double _height = 10.0;

        [ObservableProperty]
        private double _computedStartElevation;

        [ObservableProperty]
        private int _verticalDivisions = 1;

        [ObservableProperty]
        private BracingType _elevationBracing = BracingType.CrossBracing;

        public FrameProfileViewModel StartFrame { get; } = new FrameProfileViewModel();
        public FrameProfileViewModel EndFrame { get; } = new FrameProfileViewModel();

        public IStructureSegment ToDomainModel()
        {
            return new CageSegment
            {
                HostsRakeArms = this.HostsRakeArms,
                StartElevation = this.ComputedStartElevation,
                EndElevation = this.ComputedStartElevation + this.Height,
                NumberOfVerticalDivisions = this.VerticalDivisions,
                ElevationBracing = this.ElevationBracing,
                StartFrame = this.StartFrame.ToDomainModel(),
                EndFrame = this.EndFrame.ToDomainModel()
            };
        }
    }
}

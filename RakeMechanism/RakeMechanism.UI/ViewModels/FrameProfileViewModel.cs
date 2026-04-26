using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RakeMechanism.Geometry.Domain;

namespace RakeMechanism.UI.ViewModels
{
    public enum DimensionInputMethod
    {
        SideDimension,
        Radius
    }

    public enum OctagonalConnectivity
    {
        ContinueFromVertices,
        ContinueFromCorners
    }

    public partial class FrameProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsOctagonal))]
        [NotifyPropertyChangedFor(nameof(CanUseRadiusInput))]
        [NotifyPropertyChangedFor(nameof(DimensionValueLabel))]
        [NotifyPropertyChangedFor(nameof(RadiusInputHint))]
        private FrameShape _shape = FrameShape.Square;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DimensionValueLabel))]
        [NotifyPropertyChangedFor(nameof(UsesRadiusInput))]
        private DimensionInputMethod _inputMethod = DimensionInputMethod.SideDimension;

        [ObservableProperty]
        private double _dimensionValue = 5.0;

        [ObservableProperty]
        private double _cornerOffset = 1.0;

        [ObservableProperty]
        private OctagonalConnectivity _connectivity = OctagonalConnectivity.ContinueFromVertices;

        [ObservableProperty]
        private bool _closeCorners = false;

        public bool IsOctagonal => Shape == FrameShape.Octagonal;

        public bool CanUseRadiusInput => Shape == FrameShape.Square;

        public bool UsesRadiusInput => CanUseRadiusInput && InputMethod == DimensionInputMethod.Radius;

        public string DimensionValueLabel => UsesRadiusInput ? "RADIUS (m)" : "SIDE DIMENSION (m)";

        public string RadiusInputHint => "Radius input uses the square corner radius so the side is deduced consistently for cage-to-tube interfaces.";

        partial void OnShapeChanged(FrameShape value)
        {
            if (value == FrameShape.Octagonal && InputMethod == DimensionInputMethod.Radius)
            {
                InputMethod = DimensionInputMethod.SideDimension;
            }
        }

        public FrameProfile ToDomainModel()
        {
            double sideDim = DimensionValue;
            double radius = 0.0;

            if (UsesRadiusInput)
            {
                radius = DimensionValue;
                sideDim = DimensionValue * System.Math.Sqrt(2.0);
            }
            else
            {
                radius = sideDim / System.Math.Sqrt(2.0);
            }

            return new FrameProfile
            {
                Shape = this.Shape,
                SideDimension = sideDim,
                Radius = radius,
                CornerOffset = this.CornerOffset,
                ContinueFromVertices = this.Connectivity == OctagonalConnectivity.ContinueFromVertices,
                CloseCorners = this.CloseCorners
            };
        }
    }
}

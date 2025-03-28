using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.DroopModifier.Interfaces;
using ReInvented.Shared;
using ReInvented.Shared.Stores;
using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.DroopModifier.Models
{
    public class Feed : ValidatablePropertyStore
    {
        #region Default Constructor

        public Feed()
        {

        }

        #endregion

        #region Public Properties

        public int RadialSegmentsCount { get => Get<int>(); set { Set(value); UpdateRadialBeamsLocations(); } }

        public double AlphaStart { get => Get<double>(); set { Set(value); UpdateRadialBeamsLocations(); } }

        public double SegmentIncludedAngle { get => Get<double>(); private set => Set(value); }

        public double RadiusToZeroDroop { get => Get<double>(); set => Set(value); }

        public HashSet<IReading> Readings { get => Get<HashSet<IReading>>(); set => Set(value); }

        public HashSet<double> RadialBeamsLocations { get => Get<HashSet<double>>(); set => Set(value); }

        #endregion

        public bool NodeFallsOnRadialBeam(Node node, Node origin)
        {
            return RadialBeamsLocations.Any(rbAngle => Math.Abs(rbAngle - Node.PlanAngleIn360DegreesOf(node, origin)) <= Constants.Tolerance);
        }

        #region Private Helpers

        private void UpdateRadialBeamsLocations()
        {
            SegmentIncludedAngle = Constants.WholeCircleAngleInDegrees / RadialSegmentsCount;
            RadialBeamsLocations = new HashSet<double>();

            for (int i = 0; i < RadialSegmentsCount; i++)
            {
                double rbAngle = AlphaStart + (1.0 / 2.0 * SegmentIncludedAngle) + (i * SegmentIncludedAngle);
                _ = RadialBeamsLocations.Add(rbAngle == Constants.WholeCircleAngleInDegrees ? 0.0 : rbAngle);
            }
        }

        #endregion

    }
}

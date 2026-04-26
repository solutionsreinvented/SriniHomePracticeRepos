using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RakeMechanism.Geometry.Domain
{
    public enum SegmentBoundaryKind
    {
        FixedFrame,
        FlexibleTube
    }

    public sealed class SegmentBoundaryDefinition
    {
        public SegmentBoundaryDefinition(
            string description,
            SegmentBoundaryKind kind,
            double radius,
            IReadOnlyList<double>? connectivityAnglesDegrees = null)
        {
            Description = description;
            Kind = kind;
            Radius = radius;
            ConnectivityAnglesDegrees = NormalizeAngles(connectivityAnglesDegrees);
        }

        public string Description { get; }

        public SegmentBoundaryKind Kind { get; }

        public double Radius { get; }

        public IReadOnlyList<double> ConnectivityAnglesDegrees { get; }

        public int ConnectivityNodeCount => ConnectivityAnglesDegrees.Count;

        public bool AllowsAdditionalNodes => Kind == SegmentBoundaryKind.FlexibleTube;

        public string RadiusText => Radius.ToString("0.###", CultureInfo.InvariantCulture);

        public static double NormalizeAngleDegrees(double angle)
        {
            double normalized = angle % 360.0;
            return normalized < 0 ? normalized + 360.0 : normalized;
        }

        private static IReadOnlyList<double> NormalizeAngles(IReadOnlyList<double>? angles)
        {
            if (angles == null || angles.Count == 0)
            {
                return Array.Empty<double>();
            }

            var normalizedAngles = new List<double>();

            foreach (double angle in angles)
            {
                double normalized = NormalizeAngleDegrees(angle);
                bool alreadyPresent = normalizedAngles.Any(existing => Math.Abs(existing - normalized) <= 0.0001);

                if (!alreadyPresent)
                {
                    normalizedAngles.Add(normalized);
                }
            }

            return normalizedAngles;
        }
    }
}

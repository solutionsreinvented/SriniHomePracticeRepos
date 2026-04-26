using System;
using System.Collections.Generic;
using System.Linq;
using ReInvented.StaadPro.Interop.Entities;

namespace RakeMechanism.Geometry.Domain
{
    public class CentralStructureGenerator
    {
        private const double RadiusTolerance = 0.0001;
        private const double AngleTolerance = 0.001;

        public List<IStructureSegment> Segments { get; set; } = new();

        public List<Node> AllNodes { get; private set; } = new();

        public List<Beam> AllBeams { get; private set; } = new();

        public List<Plate> AllPlates { get; private set; } = new();

        public List<string> ValidationErrors { get; } = new();

        public List<string> ValidationWarnings { get; } = new();

        public IStructureSegment? RakeArmConnectionSegment { get; private set; }

        public void Validate()
        {
            ValidationErrors.Clear();
            ValidationWarnings.Clear();

            if (Segments.Count == 0)
            {
                ValidationErrors.Add("Add at least one segment before generating the central structure.");
                return;
            }

            for (int i = 0; i < Segments.Count; i++)
            {
                Segments[i].CollectValidationIssues(i, ValidationErrors, ValidationWarnings);
            }

            int rakeCarrierCount = Segments.Count(segment => segment.HostsRakeArms);

            if (rakeCarrierCount > 1)
            {
                ValidationErrors.Add("Only one segment can be reserved for future rake-arm attachment.");
            }
            else if (rakeCarrierCount == 0)
            {
                ValidationWarnings.Add("No segment is currently reserved for the future rake-arm connection phase.");
            }

            for (int i = 0; i < Segments.Count - 1; i++)
            {
                ValidateInterface(i, Segments[i], Segments[i + 1]);
            }
        }

        public void Generate()
        {
            Validate();

            if (ValidationErrors.Count > 0)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, ValidationErrors));
            }

            AllNodes = new List<Node>();
            AllBeams = new List<Beam>();
            AllPlates = new List<Plate>();
            RakeArmConnectionSegment = Segments.FirstOrDefault(segment => segment.HostsRakeArms);

            int nodeIdCounter = 0;
            int elementIdCounter = 0;
            List<Node>? previousTopNodes = null;

            for (int i = 0; i < Segments.Count; i++)
            {
                IStructureSegment segment = Segments[i];
                SegmentBoundaryDefinition? nextBoundary = i < Segments.Count - 1
                    ? Segments[i + 1].GetStartBoundaryDefinition()
                    : null;

                segment.GenerateGeometry(previousTopNodes, nextBoundary, ref nodeIdCounter, ref elementIdCounter);

                previousTopNodes = segment.TopConnectivityNodes;

                AllNodes.AddRange(segment.GeneratedNodes);
                AllBeams.AddRange(segment.GeneratedBeams);
                AllPlates.AddRange(segment.GeneratedPlates);
            }

            AllNodes = AllNodes.OrderBy(node => node.Id).ToList();
            AllBeams = AllBeams.OrderBy(beam => beam.Id).ToList();
            AllPlates = AllPlates.OrderBy(plate => plate.Id).ToList();
        }

        private void ValidateInterface(int index, IStructureSegment lowerSegment, IStructureSegment upperSegment)
        {
            SegmentBoundaryDefinition lowerBoundary = lowerSegment.GetEndBoundaryDefinition();
            SegmentBoundaryDefinition upperBoundary = upperSegment.GetStartBoundaryDefinition();
            string prefix = $"Interface between segment {index + 1} ({lowerSegment.SegmentLabel}) and segment {index + 2} ({upperSegment.SegmentLabel})";

            if (Math.Abs(lowerBoundary.Radius - upperBoundary.Radius) > RadiusTolerance)
            {
                ValidationErrors.Add(
                    $"{prefix}: boundary radii do not match ({lowerBoundary.RadiusText} m vs {upperBoundary.RadiusText} m).");
            }

            if (lowerBoundary.Kind == SegmentBoundaryKind.FlexibleTube || upperBoundary.Kind == SegmentBoundaryKind.FlexibleTube)
            {
                return;
            }

            if (lowerBoundary.ConnectivityNodeCount != upperBoundary.ConnectivityNodeCount)
            {
                ValidationErrors.Add(
                    $"{prefix}: connection node counts do not match ({lowerBoundary.ConnectivityNodeCount} vs {upperBoundary.ConnectivityNodeCount}).");
                return;
            }

            if (!AnglesMatch(lowerBoundary.ConnectivityAnglesDegrees, upperBoundary.ConnectivityAnglesDegrees))
            {
                ValidationErrors.Add(
                    $"{prefix}: connection node angles do not line up between the adjoining frame boundaries.");
            }
        }

        private static bool AnglesMatch(IReadOnlyList<double> first, IReadOnlyList<double> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            var left = first.OrderBy(angle => angle).ToArray();
            var right = second.OrderBy(angle => angle).ToArray();

            for (int i = 0; i < left.Length; i++)
            {
                if (Math.Abs(left[i] - right[i]) > AngleTolerance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

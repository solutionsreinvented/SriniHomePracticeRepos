using System;
using System.Collections.Generic;
using System.Linq;
using ReInvented.StaadPro.Interop.Entities;

namespace RakeMechanism.Geometry.Domain
{
    public class TubeSegment : IStructureSegment
    {
        public string SegmentLabel => "Tube";

        public bool HostsRakeArms { get; set; }

        public double StartElevation { get; set; }

        public double EndElevation { get; set; }

        public double StartRadius { get; set; }

        public double EndRadius { get; set; }

        public int VerticalDivisions { get; set; } = 1;

        public int CircumferentialDivisions { get; set; } = 36;

        public List<Node> BottomConnectivityNodes { get; private set; } = new();

        public List<Node> TopConnectivityNodes { get; private set; } = new();

        public List<Node> GeneratedNodes { get; private set; } = new();

        public List<Beam> GeneratedBeams { get; private set; } = new();

        public List<Plate> GeneratedPlates { get; private set; } = new();

        public SegmentBoundaryDefinition GetStartBoundaryDefinition() =>
            new("tube start ring", SegmentBoundaryKind.FlexibleTube, StartRadius);

        public SegmentBoundaryDefinition GetEndBoundaryDefinition() =>
            new("tube end ring", SegmentBoundaryKind.FlexibleTube, EndRadius);

        public void CollectValidationIssues(int segmentIndex, ICollection<string> errors, ICollection<string> warnings)
        {
            string prefix = $"Segment {segmentIndex + 1} (Tube)";

            if (EndElevation <= StartElevation)
            {
                errors.Add($"{prefix}: the segment height must be greater than zero.");
            }

            if (StartRadius <= 0 || EndRadius <= 0)
            {
                errors.Add($"{prefix}: both radii must be greater than zero.");
            }

            if (VerticalDivisions < 1)
            {
                errors.Add($"{prefix}: at least one vertical subdivision is required.");
            }

            if (CircumferentialDivisions < 3)
            {
                errors.Add($"{prefix}: at least three circumferential divisions are required.");
            }

            double plateHeight = Math.Abs(EndElevation - StartElevation) / Math.Max(VerticalDivisions, 1);
            double averageRadius = (StartRadius + EndRadius) / 2.0;
            double plateWidth = averageRadius > 0
                ? (2.0 * Math.PI * averageRadius) / Math.Max(CircumferentialDivisions, 1)
                : 0.0;

            if (plateHeight > 0 && plateWidth > 0)
            {
                double aspectRatio = Math.Max(plateHeight, plateWidth) / Math.Min(plateHeight, plateWidth);

                if (aspectRatio > 3.0)
                {
                    warnings.Add($"{prefix}: the nominal plate aspect ratio is about {aspectRatio:0.##}. Consider adjusting mesh divisions.");
                }
            }
        }

        public void GenerateGeometry(
            IReadOnlyList<Node>? previousSegmentTopNodes,
            SegmentBoundaryDefinition? nextSegmentStartBoundary,
            ref int lastUsedNodeId,
            ref int lastUsedElementId)
        {
            ResetGeneratedCollections();

            IReadOnlyList<double> bottomRequiredAngles = previousSegmentTopNodes is { Count: > 0 }
                ? ExtractAngles(previousSegmentTopNodes)
                : Array.Empty<double>();

            IReadOnlyList<double> topRequiredAngles = nextSegmentStartBoundary?.Kind == SegmentBoundaryKind.FixedFrame
                ? nextSegmentStartBoundary.ConnectivityAnglesDegrees
                : Array.Empty<double>();

            List<double> masterAngles = BuildMasterAngles(bottomRequiredAngles, topRequiredAngles, CircumferentialDivisions);
            var ringNodesByLevel = new List<List<Node>>(VerticalDivisions + 1);
            IReadOnlyDictionary<double, Node>? inheritedNodesByAngle = previousSegmentTopNodes is { Count: > 0 }
                ? BuildAngleNodeMap(previousSegmentTopNodes)
                : null;

            for (int levelIndex = 0; levelIndex <= VerticalDivisions; levelIndex++)
            {
                double ratio = (double)levelIndex / VerticalDivisions;
                double elevation = Lerp(StartElevation, EndElevation, ratio);
                double radius = Lerp(StartRadius, EndRadius, ratio);

                var ring = CreateRingNodes(
                    elevation,
                    radius,
                    masterAngles,
                    levelIndex == 0 ? inheritedNodesByAngle : null,
                    ref lastUsedNodeId);

                ringNodesByLevel.Add(ring.AllNodes);
                GeneratedNodes.AddRange(ring.NewNodes);

                if (levelIndex == 0)
                {
                    BottomConnectivityNodes = previousSegmentTopNodes is { Count: > 0 }
                        ? previousSegmentTopNodes.ToList()
                        : ring.AllNodes.ToList();
                }

                if (levelIndex == VerticalDivisions)
                {
                    TopConnectivityNodes = nextSegmentStartBoundary?.Kind == SegmentBoundaryKind.FixedFrame
                        ? SelectNodesForAngles(ring.AllNodes, masterAngles, nextSegmentStartBoundary.ConnectivityAnglesDegrees)
                        : ring.AllNodes.ToList();
                }
            }

            for (int levelIndex = 0; levelIndex < VerticalDivisions; levelIndex++)
            {
                IReadOnlyList<Node> lowerRing = ringNodesByLevel[levelIndex];
                IReadOnlyList<Node> upperRing = ringNodesByLevel[levelIndex + 1];

                for (int angleIndex = 0; angleIndex < masterAngles.Count; angleIndex++)
                {
                    Node n1 = lowerRing[angleIndex];
                    Node n2 = lowerRing[(angleIndex + 1) % masterAngles.Count];
                    Node n3 = upperRing[(angleIndex + 1) % masterAngles.Count];
                    Node n4 = upperRing[angleIndex];

                    GeneratedPlates.Add(new Plate(++lastUsedElementId, n1, n2, n3, n4));
                }
            }
        }

        private void ResetGeneratedCollections()
        {
            BottomConnectivityNodes = new List<Node>();
            TopConnectivityNodes = new List<Node>();
            GeneratedNodes = new List<Node>();
            GeneratedBeams = new List<Beam>();
            GeneratedPlates = new List<Plate>();
        }

        private static (List<Node> AllNodes, List<Node> NewNodes) CreateRingNodes(
            double elevation,
            double radius,
            IReadOnlyList<double> masterAngles,
            IReadOnlyDictionary<double, Node>? reusableNodesByAngle,
            ref int lastUsedNodeId)
        {
            var allNodes = new List<Node>(masterAngles.Count);
            var newNodes = new List<Node>(masterAngles.Count);

            foreach (double angle in masterAngles)
            {
                double roundedKey = RoundAngleKey(angle);

                if (reusableNodesByAngle != null && reusableNodesByAngle.TryGetValue(roundedKey, out Node? reusableNode))
                {
                    allNodes.Add(reusableNode);
                    continue;
                }

                double radians = angle * Math.PI / 180.0;
                var newNode = new Node(++lastUsedNodeId, radius * Math.Cos(radians), elevation, radius * Math.Sin(radians));

                allNodes.Add(newNode);
                newNodes.Add(newNode);
            }

            return (allNodes, newNodes);
        }

        private static List<double> BuildMasterAngles(
            IReadOnlyList<double> bottomRequiredAngles,
            IReadOnlyList<double> topRequiredAngles,
            int requestedDivisions)
        {
            var angles = new List<double>();

            foreach (double angle in bottomRequiredAngles.Concat(topRequiredAngles))
            {
                AddAngleIfMissing(angles, angle);
            }

            int targetCount = Math.Max(requestedDivisions, angles.Count);

            if (angles.Count == 0)
            {
                targetCount = Math.Max(targetCount, 3);

                for (int i = 0; i < targetCount; i++)
                {
                    angles.Add((360.0 / targetCount) * i);
                }

                return angles;
            }

            angles.Sort();

            while (angles.Count < targetCount)
            {
                int insertAfterIndex = FindLargestGapIndex(angles);
                double start = angles[insertAfterIndex];
                double end = angles[(insertAfterIndex + 1) % angles.Count];
                double gap = end > start ? end - start : (end + 360.0) - start;

                AddAngleIfMissing(angles, SegmentBoundaryDefinition.NormalizeAngleDegrees(start + (gap / 2.0)));
                angles.Sort();
            }

            return angles;
        }

        private static int FindLargestGapIndex(IReadOnlyList<double> angles)
        {
            int largestGapIndex = 0;
            double largestGap = -1;

            for (int i = 0; i < angles.Count; i++)
            {
                double start = angles[i];
                double end = angles[(i + 1) % angles.Count];
                double gap = end > start ? end - start : (end + 360.0) - start;

                if (gap > largestGap)
                {
                    largestGap = gap;
                    largestGapIndex = i;
                }
            }

            return largestGapIndex;
        }

        private static IReadOnlyList<double> ExtractAngles(IEnumerable<Node> nodes)
        {
            return nodes
                .Select(node => SegmentBoundaryDefinition.NormalizeAngleDegrees(Math.Atan2(node.Z, node.X) * 180.0 / Math.PI))
                .ToArray();
        }

        private static IReadOnlyDictionary<double, Node> BuildAngleNodeMap(IEnumerable<Node> nodes)
        {
            return nodes.ToDictionary(
                node => RoundAngleKey(SegmentBoundaryDefinition.NormalizeAngleDegrees(Math.Atan2(node.Z, node.X) * 180.0 / Math.PI)),
                node => node);
        }

        private static List<Node> SelectNodesForAngles(
            IReadOnlyList<Node> ringNodes,
            IReadOnlyList<double> ringAngles,
            IReadOnlyList<double> requiredAngles)
        {
            var angleNodeMap = new Dictionary<double, Node>(ringAngles.Count);

            for (int i = 0; i < ringAngles.Count; i++)
            {
                angleNodeMap[RoundAngleKey(ringAngles[i])] = ringNodes[i];
            }

            return requiredAngles
                .Select(angle => angleNodeMap[RoundAngleKey(angle)])
                .ToList();
        }

        private static void AddAngleIfMissing(ICollection<double> angles, double candidateAngle)
        {
            double normalized = SegmentBoundaryDefinition.NormalizeAngleDegrees(candidateAngle);
            bool exists = angles.Any(existing => Math.Abs(existing - normalized) <= 0.0001);

            if (!exists)
            {
                angles.Add(normalized);
            }
        }

        private static double RoundAngleKey(double angle) =>
            Math.Round(SegmentBoundaryDefinition.NormalizeAngleDegrees(angle), 6);

        private static double Lerp(double start, double end, double ratio) =>
            start + ((end - start) * ratio);
    }
}

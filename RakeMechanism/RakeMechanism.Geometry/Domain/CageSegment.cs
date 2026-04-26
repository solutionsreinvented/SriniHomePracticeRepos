using System;
using System.Collections.Generic;
using System.Linq;
using ReInvented.StaadPro.Interop.Entities;

namespace RakeMechanism.Geometry.Domain
{
    public enum BracingType
    {
        None,
        SingleDiagonal,
        CrossBracing
    }

    public class CageSegment : IStructureSegment
    {
        public string SegmentLabel => "Cage";

        public bool HostsRakeArms { get; set; }

        public double StartElevation { get; set; }

        public double EndElevation { get; set; }

        public FrameProfile StartFrame { get; set; } = new();

        public FrameProfile EndFrame { get; set; } = new();

        public int NumberOfVerticalDivisions { get; set; } = 1;

        public BracingType ElevationBracing { get; set; } = BracingType.CrossBracing;

        public List<Node> BottomConnectivityNodes { get; private set; } = new();

        public List<Node> TopConnectivityNodes { get; private set; } = new();

        public List<Node> GeneratedNodes { get; private set; } = new();

        public List<Beam> GeneratedBeams { get; private set; } = new();

        public List<Plate> GeneratedPlates { get; private set; } = new();

        public SegmentBoundaryDefinition GetStartBoundaryDefinition() =>
            StartFrame.CreateBoundaryDefinition("cage start frame");

        public SegmentBoundaryDefinition GetEndBoundaryDefinition() =>
            EndFrame.CreateBoundaryDefinition("cage end frame");

        public void CollectValidationIssues(int segmentIndex, ICollection<string> errors, ICollection<string> warnings)
        {
            string prefix = $"Segment {segmentIndex + 1} (Cage)";

            if (EndElevation <= StartElevation)
            {
                errors.Add($"{prefix}: the segment height must be greater than zero.");
            }

            if (NumberOfVerticalDivisions < 1)
            {
                errors.Add($"{prefix}: at least one vertical subdivision is required.");
            }

            StartFrame.Validate($"{prefix} start frame", errors);
            EndFrame.Validate($"{prefix} end frame", errors);

            int startCount = StartFrame.GetConnectivityNodeCount();
            int endCount = EndFrame.GetConnectivityNodeCount();

            if (!SupportsConnectivityTransition(startCount, endCount))
            {
                errors.Add($"{prefix}: connectivity transitions must keep the same node count or use a 4-to-8 / 8-to-4 split.");
            }
            else if (startCount != endCount)
            {
                warnings.Add($"{prefix}: connectivity transitions will be triangulated between {startCount} and {endCount} connection nodes.");
            }
        }

        public void GenerateGeometry(
            IReadOnlyList<Node>? previousSegmentTopNodes,
            SegmentBoundaryDefinition? nextSegmentStartBoundary,
            ref int lastUsedNodeId,
            ref int lastUsedElementId)
        {
            ResetGeneratedCollections();

            FrameGeometry? previousFrame = null;

            for (int levelIndex = 0; levelIndex <= NumberOfVerticalDivisions; levelIndex++)
            {
                double ratio = NumberOfVerticalDivisions == 0 ? 0.0 : (double)levelIndex / NumberOfVerticalDivisions;
                double elevation = StartElevation + ((EndElevation - StartElevation) * ratio);
                FrameProfile profile = InterpolateProfile(StartFrame, EndFrame, ratio);
                FrameGeometry currentFrame = profile.CreateFrameGeometry(elevation, ref lastUsedNodeId);

                if (levelIndex == 0 && previousSegmentTopNodes != null && previousSegmentTopNodes.Count > 0)
                {
                    currentFrame.ReuseConnectivityNodes(previousSegmentTopNodes);
                }

                GeneratedNodes.AddRange(currentFrame.NewlyGeneratedNodes);
                GenerateRingBeams(currentFrame, ref lastUsedElementId);

                if (levelIndex == 0)
                {
                    BottomConnectivityNodes = currentFrame.ConnectivityNodes.ToList();
                }
                else if (previousFrame != null)
                {
                    GenerateVerticalMembers(previousFrame.ConnectivityNodes, currentFrame.ConnectivityNodes, ref lastUsedElementId);
                }

                if (levelIndex == NumberOfVerticalDivisions)
                {
                    TopConnectivityNodes = currentFrame.ConnectivityNodes.ToList();
                }

                previousFrame = currentFrame;
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

        private static bool SupportsConnectivityTransition(int startCount, int endCount)
        {
            return startCount == endCount || startCount % endCount == 0 || endCount % startCount == 0;
        }

        private FrameProfile InterpolateProfile(FrameProfile start, FrameProfile end, double ratio)
        {
            double startSide = start.ResolveSideDimension();
            double endSide = end.ResolveSideDimension();
            double startOffset = start.IsEffectivelySquare ? 0.0 : start.CornerOffset;
            double endOffset = end.IsEffectivelySquare ? 0.0 : end.CornerOffset;

            return new FrameProfile
            {
                Shape = start.IsEffectivelySquare && end.IsEffectivelySquare ? FrameShape.Square : FrameShape.Octagonal,
                SideDimension = Lerp(startSide, endSide, ratio),
                Radius = Lerp(start.Radius, end.Radius, ratio),
                CornerOffset = Lerp(startOffset, endOffset, ratio),
                ContinueFromVertices = ratio < 1.0 ? start.ContinueFromVertices : end.ContinueFromVertices,
                CloseCorners = ratio < 1.0 ? start.CloseCorners : end.CloseCorners
            };
        }

        private void GenerateRingBeams(FrameGeometry frame, ref int lastUsedElementId)
        {
            for (int i = 0; i < frame.PerimeterNodes.Count; i++)
            {
                Node startNode = frame.PerimeterNodes[i];
                Node endNode = frame.PerimeterNodes[(i + 1) % frame.PerimeterNodes.Count];

                GeneratedBeams.Add(new Beam(++lastUsedElementId, startNode, endNode));
            }

            if (!frame.Profile.CloseCorners || frame.CornerNodes.Count == 0)
            {
                return;
            }

            foreach (Node cornerNode in frame.CornerNodes)
            {
                var nearestPerimeterNodes = frame.PerimeterNodes
                    .OrderBy(node => Node.DistanceBetweenNodes(node, cornerNode))
                    .Take(2)
                    .ToArray();

                foreach (Node perimeterNode in nearestPerimeterNodes)
                {
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, cornerNode, perimeterNode));
                }
            }
        }

        private void GenerateVerticalMembers(IReadOnlyList<Node> lowerNodes, IReadOnlyList<Node> upperNodes, ref int lastUsedElementId)
        {
            if (lowerNodes.Count == upperNodes.Count)
            {
                GenerateEqualNodeColumns(lowerNodes, upperNodes, ref lastUsedElementId);
                return;
            }

            if (upperNodes.Count > lowerNodes.Count && upperNodes.Count % lowerNodes.Count == 0)
            {
                GenerateExpandedTransition(lowerNodes, upperNodes, ref lastUsedElementId);
                return;
            }

            if (lowerNodes.Count > upperNodes.Count && lowerNodes.Count % upperNodes.Count == 0)
            {
                GenerateContractedTransition(lowerNodes, upperNodes, ref lastUsedElementId);
                return;
            }

            throw new InvalidOperationException(
                $"Unsupported cage connectivity transition from {lowerNodes.Count} nodes to {upperNodes.Count} nodes.");
        }

        private void GenerateEqualNodeColumns(IReadOnlyList<Node> lowerNodes, IReadOnlyList<Node> upperNodes, ref int lastUsedElementId)
        {
            for (int i = 0; i < lowerNodes.Count; i++)
            {
                GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[i], upperNodes[i]));
            }

            if (ElevationBracing == BracingType.None || lowerNodes.Count < 2)
            {
                return;
            }

            for (int i = 0; i < lowerNodes.Count; i++)
            {
                int nextIndex = (i + 1) % lowerNodes.Count;

                if (ElevationBracing == BracingType.SingleDiagonal)
                {
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[i], upperNodes[nextIndex]));
                }
                else if (ElevationBracing == BracingType.CrossBracing)
                {
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[i], upperNodes[nextIndex]));
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[nextIndex], upperNodes[i]));
                }
            }
        }

        private void GenerateExpandedTransition(IReadOnlyList<Node> lowerNodes, IReadOnlyList<Node> upperNodes, ref int lastUsedElementId)
        {
            int ratio = upperNodes.Count / lowerNodes.Count;

            for (int i = 0; i < lowerNodes.Count; i++)
            {
                int primaryUpperIndex = i * ratio;
                int nextLowerIndex = (i + 1) % lowerNodes.Count;

                GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[i], upperNodes[primaryUpperIndex]));

                for (int j = 1; j < ratio; j++)
                {
                    Node upperTransitionNode = upperNodes[primaryUpperIndex + j];
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[i], upperTransitionNode));
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[nextLowerIndex], upperTransitionNode));
                }
            }
        }

        private void GenerateContractedTransition(IReadOnlyList<Node> lowerNodes, IReadOnlyList<Node> upperNodes, ref int lastUsedElementId)
        {
            int ratio = lowerNodes.Count / upperNodes.Count;

            for (int i = 0; i < upperNodes.Count; i++)
            {
                int primaryLowerIndex = i * ratio;
                int nextUpperIndex = (i + 1) % upperNodes.Count;

                GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerNodes[primaryLowerIndex], upperNodes[i]));

                for (int j = 1; j < ratio; j++)
                {
                    Node lowerTransitionNode = lowerNodes[primaryLowerIndex + j];
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerTransitionNode, upperNodes[i]));
                    GeneratedBeams.Add(new Beam(++lastUsedElementId, lowerTransitionNode, upperNodes[nextUpperIndex]));
                }
            }
        }

        private static double Lerp(double start, double end, double ratio) =>
            start + ((end - start) * ratio);
    }
}

using System.Collections.Generic;
using ReInvented.StaadPro.Interop.Entities;

namespace RakeMechanism.Geometry.Domain
{
    public interface IStructureSegment
    {
        string SegmentLabel { get; }

        bool HostsRakeArms { get; }

        double StartElevation { get; }

        double EndElevation { get; }

        List<Node> BottomConnectivityNodes { get; }

        List<Node> TopConnectivityNodes { get; }

        List<Node> GeneratedNodes { get; }

        List<Beam> GeneratedBeams { get; }

        List<Plate> GeneratedPlates { get; }

        SegmentBoundaryDefinition GetStartBoundaryDefinition();

        SegmentBoundaryDefinition GetEndBoundaryDefinition();

        void CollectValidationIssues(int segmentIndex, ICollection<string> errors, ICollection<string> warnings);

        void GenerateGeometry(
            IReadOnlyList<Node>? previousSegmentTopNodes,
            SegmentBoundaryDefinition? nextSegmentStartBoundary,
            ref int lastUsedNodeId,
            ref int lastUsedElementId);
    }
}

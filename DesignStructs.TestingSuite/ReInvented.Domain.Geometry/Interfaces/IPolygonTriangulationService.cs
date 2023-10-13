using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.Domain.Geometry.Enums;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Interfaces
{
    public interface IPolygonTriangulationService
    {
        TriangleSplitMethod TriangleSplitMethod { get; }

        (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, int currentNodeId = 0, int currentPlateId = 0, double maximumDimension = 0.0);
    }
}

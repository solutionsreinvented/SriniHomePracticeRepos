using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.Geometry.Enums;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Interfaces
{
    public interface IPolygonTriangulationService
    {
        TriangleSplitMethod TriangleSplitMethod { get; }

        (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, int currentNodeId = 0, int currentPlateId = 0, double maximumDimension = 0.0);
    }
}

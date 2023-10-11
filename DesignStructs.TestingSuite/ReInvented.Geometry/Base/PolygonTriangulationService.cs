using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.Geometry.Enums;
using ReInvented.Geometry.Interfaces;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Base
{
    public abstract class PolygonTriangulationService : IPolygonTriangulationService
    {
        public PolygonTriangulationService(TriangleSplitMethod triangleSplitMethod = TriangleSplitMethod.PerpendicularNode)
        {
            TriangleSplitMethod = triangleSplitMethod;
        }

        public TriangleSplitMethod TriangleSplitMethod { get; private set; }

        public abstract (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, int currentNodeId = 0,
                                                                                            int currentPlateId = 0, double maximumDimension = 0.0);
    }
}

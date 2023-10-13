using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.Geometry.Enums;
using ReInvented.Domain.Geometry.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Services
{
    public class TriangulaPlatesGenerationService
    {
        public static Plate GenerateSinglePlate(Triangle triangle, int currentPlateId)
        {
            return new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], triangle.Vertices.Last());
        }
        public static (Node AdditionalNode, HashSet<Plate> Plates) GenerateSplitPlates(Triangle triangle, int currentNodeId, int currentPlateId, TriangleSplitMethod splitMethod = TriangleSplitMethod.PerpendicularNode)
        {
            Node intermediateNode;
            HashSet<Plate> plates = new HashSet<Plate>();

            if (splitMethod == TriangleSplitMethod.PerpendicularNode)
            {
                intermediateNode = triangle.PerpendicularNodes[1];
            }
            else
            {
                intermediateNode = triangle.MidNodes.Last();
            }

            /// TODO: This is directly resulting in infinite loop if the frustum is just a plane.
            ///intermediateNode = AdjustIntermediateNodeCoordinatesForConicalSurface(conicalPolygon, intermediateNode);

            intermediateNode.Id = ++currentNodeId;

            plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], intermediateNode));
            plates.Add(new Plate(++currentPlateId, intermediateNode, triangle.Vertices[1], triangle.Vertices.Last()));

            return (intermediateNode, plates);
        }
    }
}

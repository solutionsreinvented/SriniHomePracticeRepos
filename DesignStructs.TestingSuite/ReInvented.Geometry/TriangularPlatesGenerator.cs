using System.Collections.Generic;
using System.Linq;

using ReInvented.Geometry.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry
{
    public class TriangularPlatesGenerator
    {
        /// This function is really awkward so it needs to be further checked and properly designed. Not used at the moment.
        public static (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates, List<Node> RemainingNodes, int CurrentNodeId, int CurrentPlateId) Generate(Triangle triangle, List<Node> remainingNodes, int currentNodeId, int currentPlateId, double maxDimension, int startVertexIndex)
        {
            HashSet<Node> additionalNodes = new HashSet<Node>();
            HashSet<Plate> plates = new HashSet<Plate>();

            if (triangle.Sides.Last().Length <= maxDimension)
            {
                plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], triangle.Vertices.Last()));

                remainingNodes.RemoveAt(startVertexIndex + 1);
            }
            else
            {
                Node intermediateNode = triangle.PerpendicularNodes[1];

                intermediateNode.Id = ++currentNodeId;

                additionalNodes.Add(intermediateNode);

                plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], intermediateNode));
                plates.Add(new Plate(++currentPlateId, intermediateNode, triangle.Vertices[1], triangle.Vertices.Last()));

                remainingNodes.RemoveAt(startVertexIndex + 1);
                remainingNodes.Insert(startVertexIndex + 1, intermediateNode);
            }

            return (additionalNodes, plates, remainingNodes, currentNodeId, currentPlateId);
        }
    }
}

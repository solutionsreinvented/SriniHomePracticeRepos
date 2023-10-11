using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class SolidCircularPlatePointsProvider
    {
        ///TODO: Not Working
        public static List<Node> GetPoints(double radius, int nPoints = 8, double yCoordinate = 0.0, double maximumDimension = 0.0, int currentNodeId = 0)
        {
            List<Node> nodesOnCircle = CirclePointsGenerator.GetPoints(radius, nPoints, yCoordinate, currentNodeId);

            currentNodeId = nodesOnCircle.OrderBy(n => n.Id).Last().Id;

            currentNodeId += 1;

            /// TODO: This should be an input
            Node origin = new Node(currentNodeId, 0.0, 0.0, 0.0);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Node.LeastDistanceBetweenNodes(nodesOnCircle);
            }

            List<Node> closingPoints = IntermediateNodesGenerator.GenerateNodes(origin, nodesOnCircle.First(), maximumDimension, currentNodeId);

            List<Node> polygonNodes = closingPoints.Union(nodesOnCircle).ToList();

            polygonNodes.Insert(0, origin);
            
            //polygonNodes.Add(nodesOnCircle.First());

            //for (int i = closingPoints.Count - 1; i >= 0; i--)
            //{
            //    polygonNodes.Add(closingPoints[i]);
            //}

            //polygonNodes.Add(origin);

            return polygonNodes;
        }
    }

}

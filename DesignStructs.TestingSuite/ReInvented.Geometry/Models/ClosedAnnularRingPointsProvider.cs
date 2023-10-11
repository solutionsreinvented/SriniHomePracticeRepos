using System;
using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class ClosedAnnularRingPointsProvider
    {
        public static List<Node> GetPoints(double outerRadius, double innerRadius, int nOuterPoints = 8, int nInnerPoints = 8, 
                                           double yOuter = 0.0, double yInner = 0.0, double maximumDimension = 0.0, int currentNodeId = 0)
        {
            List<Node> outerNodes = CirclePointsGenerator.GetPoints(outerRadius, nOuterPoints, yOuter, currentNodeId);
            List<Node> innerNodes = CirclePointsGenerator.GetPoints(innerRadius, nInnerPoints, yInner, outerNodes.Last().Id);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(outerNodes), Node.LeastDistanceBetweenNodes(innerNodes));
            }

            List<Node> closingPoints = IntermediateNodesGenerator.GenerateNodes(outerNodes.First(), innerNodes.First(), maximumDimension, innerNodes.Last().Id);

            List<Node> polygonNodes = outerNodes.Take(1).Union(closingPoints).Union(innerNodes).ToList();
            
            polygonNodes.Add(innerNodes.First());

            for (int i = closingPoints.Count - 1; i >= 0; i--)
            {
                polygonNodes.Add(closingPoints[i]);
            }

            polygonNodes.Add(outerNodes.First());

            for (int i = outerNodes.Count - 1; i > 0; i--)
            {
                polygonNodes.Add(outerNodes[i]);
            }

            return polygonNodes;
        }
    }

}

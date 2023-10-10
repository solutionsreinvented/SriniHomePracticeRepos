using System;
using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class OpenAnnularRingPointsProvider
    {
        public static List<Node> GetPoints(double outerRadius, double innerRadius, int nOuterPoints = 8, int nInnerPoints = 8, double yOuter = 0.0, double yInner = 0.0, double maximumDimension = 0.0)
        {
            var currentNodeId = 0;

            List<Node> outerNodes = CirclePointsGenerator.GetPoints(outerRadius, nOuterPoints, yOuter, currentNodeId);
            List<Node> innerNodes = CirclePointsGenerator.GetPoints(innerRadius, nInnerPoints, yInner, outerNodes.Last().Id);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(outerNodes), Node.LeastDistanceBetweenNodes(innerNodes));
            }

            List<Node> startClosingPoints = IntermediateNodesGenerator.GenerateNodes(outerNodes.First(), innerNodes.First(), maximumDimension, innerNodes.Last().Id);
            List<Node> endClosingPoints = IntermediateNodesGenerator.GenerateNodes(innerNodes.Last(), outerNodes.Last(), maximumDimension, startClosingPoints.Last().Id);

            List<Node> nodes = outerNodes.Take(1).Union(startClosingPoints).Union(innerNodes).Union(endClosingPoints).ToList();

            for (int i = outerNodes.Count - 1; i > 0; i--)
            {
                nodes.Add(outerNodes[i]);
            }

            return nodes;
        }
    }
    public class ClosedAnnularRingPointsProvider
    {
        /// TODO: Points generation is not implemented yet.
        public static List<Node> GetPoints(double outerRadius, double innerRadius, int nOuterPoints = 8, int nInnerPoints = 8, double yOuter = 0.0, double yInner = 0.0, double maximumDimension = 0.0)
        {
            var currentNodeId = 0;

            List<Node> outerNodes = CirclePointsGenerator.GetPoints(outerRadius, nOuterPoints, yOuter, currentNodeId);
            List<Node> innerNodes = CirclePointsGenerator.GetPoints(innerRadius, nInnerPoints, yInner, outerNodes.Last().Id);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(outerNodes), Node.LeastDistanceBetweenNodes(innerNodes));
            }

            List<Node> startClosingPoints = IntermediateNodesGenerator.GenerateNodes(outerNodes.First(), innerNodes.First(), maximumDimension, innerNodes.Last().Id);
            List<Node> endClosingPoints = IntermediateNodesGenerator.GenerateNodes(innerNodes.Last(), outerNodes.Last(), maximumDimension, startClosingPoints.Last().Id);

            List<Node> nodes = outerNodes.Take(1).Union(startClosingPoints).Union(innerNodes).Union(endClosingPoints).ToList();

            for (int i = outerNodes.Count - 1; i > 0; i--)
            {
                nodes.Add(outerNodes[i]);
            }

            return nodes;
        }
    }

}

using System;
using System.Linq;
using System.Collections.Generic;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class PolygonPointsProvider
    {
        public static HashSet<Node> GetPoints()
        {
            return new HashSet<Node>()
            {
                new Node(1, 2.5, 3.5, 0),
                new Node(2, 0, 0, 0),
                new Node(3, 0, 5, 0),
                new Node(4, -5, 10, 0),
                new Node(5, 0, 15, 0),
                new Node(6, 0, 20, 0),
                new Node(7, 5, 25, 0),
                new Node(8, 10, 20, 0),
                new Node(9, 15, 17.5, 0),
                new Node(10, 20, 12.5, 0),
                new Node(11, 12.5, 7.5, 0),
                new Node(12, 10, 0, 0)
            };
        }
    }

    public class IntermediateNodesGenerator
    {
        /// TODO: Move this to the Node class
        public static List<Node> GenerateNodes(Node startNode, Node endNode, double spacing, int currentNodeId = 0)
        {
            List<Node> nodes = new List<Node>();
            double startNodeToEndNodeDistance = Node.DistanceBetweenNodes(startNode, endNode);

            if (spacing != 0 && spacing < startNodeToEndNodeDistance)
            {
                double divisions = (startNodeToEndNodeDistance / spacing).Ceiling(1);
                double adjustedSpacing = startNodeToEndNodeDistance / divisions;

                for (int i = 1; i < divisions; i++)
                {
                    currentNodeId += 1;
                    Node node = Node.CreateNewNodeByDistance(startNode, endNode, i * adjustedSpacing);

                    node.Id = currentNodeId;
                    nodes.Add(node);
                }
            }

            return nodes;
        }
    }


    public class OpenAnnularRingPointsProvider
    {
        public static List<Node> GetPoints(double outerRadius, double innerRadius, int nOuterPoints = 8, int nInnerPoints = 8, double maximumDimension = 0.0)
        {
            var currentNodeId = 0;

            List<Node> outerNodes = CirclePointsProvider.GetPoints(outerRadius, nOuterPoints, 0.0, currentNodeId);
            List<Node> innerNodes = CirclePointsProvider.GetPoints(innerRadius, nInnerPoints, -1.6, outerNodes.Last().Id);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(outerNodes), Node.LeastDistanceBetweenNodes(innerNodes));
            }

            List<Node> startClosingPoints = IntermediateNodesGenerator.GenerateNodes(outerNodes.First(), innerNodes.First(), maximumDimension, innerNodes.Last().Id);
            List<Node> endClosingPoints = IntermediateNodesGenerator.GenerateNodes(innerNodes.Last(), outerNodes.Last(), maximumDimension, startClosingPoints.Last().Id);

            List<Node> nodes = outerNodes.Take(1).Union(startClosingPoints).Union(innerNodes).Union(endClosingPoints).ToList();

            for (int i = outerNodes.Count - 1; i >= 0; i--)
            {
                nodes.Add(outerNodes[i]);
            }

            return nodes;
        }
    }

    public class OpenAnnularRingPointsProviderPrevious
    {
        public static List<Node> GetPoints(double outerRadius, double innerRadius, double maximumDimension = 0.0, int nOuterPoints = 8, int nInnerPoints = 8)
        {
            List<Node> nodes = new List<Node>();

            //double dThetaOuter = 360.0 / nOuterPoints;
            //double dThetaInner = 360.0 / nInnerPoints;

            var currentNodeId = 0;

            List<Node> outerNodes = CirclePointsProvider.GetPoints(outerRadius, nOuterPoints, 0.0, currentNodeId);
            List<Node> innerNodes = CirclePointsProvider.GetPoints(innerRadius, nInnerPoints, -1.6, outerNodes.Last().Id);

            //for (int i = 0; i < nOuterPoints; i++)
            //{
            //    currentNodeId += 1;
            //    double x = outerRadius * Math.Cos((i * dThetaOuter).ToRadians());
            //    double y = 0.0;
            //    double z = outerRadius * Math.Sin((i * dThetaOuter).ToRadians());

            //    nodes.Add(new Node(currentNodeId, x, y, z));
            //}
            //for (int i = nInnerPoints - 1; i >= 0; i--)
            //{
            //    currentNodeId += 1;
            //    double x = innerRadius * Math.Cos((i * dThetaInner).ToRadians());
            //    double y = -1.6;
            //    double z = innerRadius * Math.Sin((i * dThetaInner).ToRadians());

            //    nodes.Add(new Node(currentNodeId, x, y, z));
            //}

            return outerNodes.Union(innerNodes).ToList();
        }
    }

    public class CirclePointsProvider
    {
        public static List<Node> GetPoints(double radius, int points, double yCoordinate = 0.0, int currentNodeId = 0)
        {
            List<Node> nodes = new List<Node>();

            double dTheta = 360.0 / points;

            for (int i = 0; i < points; i++)
            {
                currentNodeId += 1;
                double xCoordinate = radius * Math.Cos((i * dTheta).ToRadians());
                double zCoordinate = radius * Math.Sin((i * dTheta).ToRadians());

                nodes.Add(new Node(currentNodeId, xCoordinate, yCoordinate, zCoordinate));
            }

            return nodes;
        }
    }

}

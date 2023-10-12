using System;
using System.Collections.Generic;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Helpers
{
    public class NodeHelpers
    {
        public static double AngleIn360Degrees(Node reference, Node target)
        {
            double angleInDegrees = Math.Atan2(target.Z - reference.Z, target.X - reference.X).Degrees() % 360.0;

            if (angleInDegrees < 0)
            {
                angleInDegrees += 360.0;
            }

            return angleInDegrees;
        }

        public static List<Node> GenerateIntermediateNodes(Node startNode, Node endNode, double spacing, int currentNodeId = 0)
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
}

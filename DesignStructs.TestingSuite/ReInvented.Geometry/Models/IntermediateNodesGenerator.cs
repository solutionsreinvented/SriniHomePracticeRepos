using System.Collections.Generic;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
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

}

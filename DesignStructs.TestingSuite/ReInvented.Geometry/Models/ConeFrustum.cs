using System;
using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.Geometry.Interfaces;

namespace ReInvented.Geometry.Models
{
    public class ConeFrustum
    {
        public ConeFrustum(List<Node> startCircleNodes, List<Node> endCircleNodes, bool edgesAreOpen = false)
        {
            StartCircleNodes = startCircleNodes;
            EndCircleNodes = endCircleNodes;
            EdgesAreOpen = edgesAreOpen;
        }

        public List<Node> StartCircleNodes { get; private set; }

        public List<Node> EndCircleNodes { get; private set; }

        public bool EdgesAreOpen { get; private set; }


        public IPolygon GetPolygon(double maximumDimension = 0.0)
        {
            Node startCenter = Circle.GetCenter(StartCircleNodes);
            Node endCenter = Circle.GetCenter(EndCircleNodes);

            var startRadius = Node.Radius(StartCircleNodes.First(), startCenter);
            var endRadius = Node.Radius(EndCircleNodes.First(), endCenter);

            List<Node> polygonNodes;

            int currentNodeId = StartCircleNodes.Union(EndCircleNodes).OrderBy(n => n.Id).Last().Id;

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(StartCircleNodes), Node.LeastDistanceBetweenNodes(EndCircleNodes));
            }

            if (EdgesAreOpen)
            {
                List<Node> startClosingPoints = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.First(), EndCircleNodes.First(), maximumDimension, currentNodeId);
                List<Node> endClosingPoints = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.Last(), EndCircleNodes.Last(), maximumDimension, startClosingPoints.Last().Id);

                polygonNodes = StartCircleNodes.Take(1).Union(startClosingPoints).Union(EndCircleNodes).ToList();

                for (int i = endClosingPoints.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(endClosingPoints[i]);
                }

                for (int i = StartCircleNodes.Count - 1; i > 0; i--)
                {
                    polygonNodes.Add(StartCircleNodes[i]);
                }
            }
            else
            {

                List<Node> closingPoints = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.First(), EndCircleNodes.First(), maximumDimension, currentNodeId);

                polygonNodes = StartCircleNodes.Take(1).Union(closingPoints).Union(EndCircleNodes).ToList();

                polygonNodes.Add(EndCircleNodes.First());

                for (int i = closingPoints.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(closingPoints[i]);
                }

                polygonNodes.Add(StartCircleNodes.First());

                for (int i = StartCircleNodes.Count - 1; i > 0; i--)
                {
                    polygonNodes.Add(StartCircleNodes[i]);
                }
            }


            ConicalPolygon polygon = new ConicalPolygon(polygonNodes, startRadius, endRadius, startCenter.Y, endCenter.Y);

            return polygon;
        }
    }

}

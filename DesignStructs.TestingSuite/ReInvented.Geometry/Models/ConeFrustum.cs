using System;
using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.Geometry.Interfaces;

namespace ReInvented.Geometry.Models
{
    public class ConeFrustum
    {
        #region Parameterized Constructor

        public ConeFrustum(List<Node> startCircleNodes, List<Node> endCircleNodes, MeshFormation formation = MeshFormation.Clockwise, bool edgesAreOpen = false)
        {
            StartCircleNodes = startCircleNodes;
            EndCircleNodes = endCircleNodes;
            EdgesAreOpen = edgesAreOpen;
            Formation = formation;
        }

        #endregion

        #region Public Properties

        public List<Node> StartCircleNodes { get; private set; }

        public List<Node> EndCircleNodes { get; private set; }

        public bool EdgesAreOpen { get; private set; }

        public MeshFormation Formation { get; private set; }

        #endregion

        #region Public Functions

        public IPolygon GetPolygon(double maximumDimension = 0.0)
        {
            Node startCenter = Circle.GetCenter(StartCircleNodes);
            Node endCenter = Circle.GetCenter(EndCircleNodes);

            double startRadius = Node.Radius(StartCircleNodes.First(), startCenter);
            double endRadius = Node.Radius(EndCircleNodes.First(), endCenter);

            List<Node> polygonNodes;

            int currentNodeId = StartCircleNodes.Union(EndCircleNodes).OrderBy(n => n.Id).Last().Id;

            if (maximumDimension == 0.0)
            {
                maximumDimension = Math.Min(Node.LeastDistanceBetweenNodes(StartCircleNodes), Node.LeastDistanceBetweenNodes(EndCircleNodes));
            }

            if (EdgesAreOpen)
            {
                List<Node> startEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.First(), EndCircleNodes.First(), maximumDimension, currentNodeId);
                List<Node> endEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.Last(), EndCircleNodes.Last(), maximumDimension, startEdgeClosingNodes.Last().Id);

                polygonNodes = GeneratePolygonNodes(StartCircleNodes, EndCircleNodes, startEdgeClosingNodes, endEdgeClosingNodes, Formation);
            }
            else
            {
                List<Node> closingNodes = IntermediateNodesGenerator.GenerateNodes(StartCircleNodes.First(), EndCircleNodes.First(), maximumDimension, currentNodeId);

                polygonNodes = GeneratePolygonNodes(StartCircleNodes, EndCircleNodes, closingNodes, Formation);
            }

            ConicalPolygon polygon = new ConicalPolygon(polygonNodes, startCenter, endCenter, startRadius, endRadius, startCenter.Y, endCenter.Y);

            return polygon;
        } 

        #endregion

        #region Private Helpers

        private static List<Node> GeneratePolygonNodes(List<Node> startCircleNodes, List<Node> endCircleNodes, List<Node> startEdgeClosingNodes, List<Node> endEdgeClosingNodes, MeshFormation formation = MeshFormation.Clockwise)
        {
            List<Node> polygonNodes;

            if (formation == MeshFormation.Clockwise)
            {
                polygonNodes = startCircleNodes.Take(1).Union(startEdgeClosingNodes).Union(endCircleNodes).ToList();

                for (int i = endEdgeClosingNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(endEdgeClosingNodes[i]);
                }

                for (int i = startCircleNodes.Count - 1; i > 0; i--)
                {
                    polygonNodes.Add(startCircleNodes[i]);
                }
            }
            else
            {
                polygonNodes = startCircleNodes.Union(endEdgeClosingNodes).ToList();

                for (int i = endCircleNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(endCircleNodes[i]);
                }

                for (int i = startEdgeClosingNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(startEdgeClosingNodes[i]);
                }
            }

            return polygonNodes;
        }

        private static List<Node> GeneratePolygonNodes(List<Node> startCircleNodes, List<Node> endCircleNodes, List<Node> closingNodes, MeshFormation formation = MeshFormation.Clockwise)
        {
            List<Node> polygonNodes;

            if (formation == MeshFormation.Clockwise)
            {
                polygonNodes = startCircleNodes.Take(1).Union(closingNodes).Union(endCircleNodes).ToList();

                polygonNodes.Add(endCircleNodes.First());

                for (int i = closingNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(closingNodes[i]);
                }

                polygonNodes.Add(startCircleNodes.First());

                for (int i = startCircleNodes.Count - 1; i > 0; i--)
                {
                    polygonNodes.Add(startCircleNodes[i]);
                }
            }
            else
            {
                polygonNodes = startCircleNodes;
                polygonNodes.Add(startCircleNodes.First());
                polygonNodes.AddRange(closingNodes);
                polygonNodes.Add(endCircleNodes.First());

                for (int i = endCircleNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(endCircleNodes[i]);
                }

                for (int i = closingNodes.Count - 1; i >= 0; i--)
                {
                    polygonNodes.Add(closingNodes[i]);
                }
            }

            return polygonNodes;
        }

        #endregion

    }

}

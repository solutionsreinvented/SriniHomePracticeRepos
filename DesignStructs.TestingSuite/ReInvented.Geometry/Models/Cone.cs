using System;
using System.Linq;
using System.Collections.Generic;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.Geometry.Interfaces;
using ReInvented.Shared;

namespace ReInvented.Geometry.Models
{
    public class Cone
    {
        public Cone(Node apex, List<Node> endCircleNodes, int nPolygons = 2, bool edgesAreOpen = false)
        {
            Apex = apex;
            CircleNodes = endCircleNodes;
            NumberOfPolygons = nPolygons;
            EdgesAreOpen = edgesAreOpen;
        }

        public Node Apex { get; private set; }

        public List<Node> CircleNodes { get; private set; }

        public bool EdgesAreOpen { get; private set; }

        public int NumberOfPolygons { get; private set; }

        public HashSet<IPolygon> GetPolygons(double maximumDimension = 0.0)
        {
            HashSet<IPolygon> polygons = new HashSet<IPolygon>();

            Node startCenter = Apex;
            Node endCenter = Circle.GetCenter(CircleNodes);

            double startRadius = Node.Radius(Apex, startCenter);
            double endRadius = Node.Radius(CircleNodes.First(), endCenter);

            int currentNodeId = Math.Max(Apex.Id, CircleNodes.OrderBy(n => n.Id).Last().Id);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Node.LeastDistanceBetweenNodes(CircleNodes);
            }

            int nodesToSkip = (int)(CircleNodes.Count() / NumberOfPolygons).Ceiling(1);

            if (EdgesAreOpen)
            {

                List<Node> startEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(Apex, CircleNodes.First(), maximumDimension, currentNodeId); ;
                List<Node> endEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(Apex, CircleNodes.Last(), maximumDimension, startEdgeClosingNodes.Last().Id); ;

                currentNodeId = endEdgeClosingNodes.OrderBy(n => n.Id).Last().Id;

                for (int iPolygon = 0; iPolygon < NumberOfPolygons; iPolygon++)
                {
                    List<Node> polygonCurveNodes;

                    if (iPolygon < NumberOfPolygons - 1)
                    {
                        polygonCurveNodes = CircleNodes.Skip(iPolygon * nodesToSkip - 1).Take(nodesToSkip).ToList();
                    }
                    else
                    {
                        polygonCurveNodes = CircleNodes.Skip(iPolygon * nodesToSkip - 1).ToList();
                    }

                    List<Node> intermediateEdgeClosingNodes;

                    if (iPolygon < NumberOfPolygons - 1)
                    {
                        intermediateEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(Apex, polygonCurveNodes.Last(), maximumDimension, currentNodeId);
                    }
                    else
                    {
                        intermediateEdgeClosingNodes = endEdgeClosingNodes;
                    }


                    List<Node> polygonNodes = new List<Node>() { Apex };
                    polygonNodes.AddRange(startEdgeClosingNodes);
                    polygonNodes.AddRange(polygonCurveNodes);

                    for (int i = intermediateEdgeClosingNodes.Count - 1; i >= 0; i--)
                    {
                        polygonNodes.Add(intermediateEdgeClosingNodes[i]);
                    }

                    ConicalPolygon polygon = new ConicalPolygon(polygonNodes, startCenter, endCenter, startRadius, endRadius, startCenter.Y, endCenter.Y);

                    polygons.Add(polygon);


                    startEdgeClosingNodes = intermediateEdgeClosingNodes;
                    currentNodeId = Math.Max(currentNodeId, intermediateEdgeClosingNodes.OrderBy(n => n.Id).Last().Id);
                }
            }
            else
            {

                List<Node> commonEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(Apex, CircleNodes.First(), maximumDimension, currentNodeId);
                List<Node> startEdgeClosingNodes = commonEdgeClosingNodes;

                currentNodeId = commonEdgeClosingNodes.OrderBy(n => n.Id).Last().Id;

                for (int iPolygon = 0; iPolygon < NumberOfPolygons; iPolygon++)
                {
                    List<Node> polygonCurveNodes;

                    if (iPolygon < NumberOfPolygons - 1)
                    {
                        polygonCurveNodes = CircleNodes.Skip(iPolygon * nodesToSkip - 1).Take(nodesToSkip).ToList();
                    }
                    else
                    {
                        polygonCurveNodes = CircleNodes.Skip(iPolygon * nodesToSkip - 1).ToList();
                        polygonCurveNodes.Add(CircleNodes.First());
                    }

                    Node polygonCurveStartNode = polygonCurveNodes.First();
                    Node polygonCurveEndNode = polygonCurveNodes.Last();

                    List<Node> intermediateEdgeClosingNodes;

                    if (iPolygon < NumberOfPolygons - 1)
                    {
                        intermediateEdgeClosingNodes = IntermediateNodesGenerator.GenerateNodes(Apex, polygonCurveEndNode, maximumDimension);
                    }
                    else
                    {
                        intermediateEdgeClosingNodes = commonEdgeClosingNodes;
                    }


                    List<Node> polygonNodes = new List<Node>() { Apex };
                    polygonNodes.Union(startEdgeClosingNodes).Union(polygonCurveNodes);

                    for (int i = intermediateEdgeClosingNodes.Count - 1; i >= 0; i--)
                    {
                        polygonNodes.Add(intermediateEdgeClosingNodes[i]);
                    }

                    ConicalPolygon polygon = new ConicalPolygon(polygonNodes, startCenter, endCenter, startRadius, endRadius, startCenter.Y, endCenter.Y);

                    polygons.Add(polygon);


                    startEdgeClosingNodes = intermediateEdgeClosingNodes;
                    currentNodeId = Math.Max(currentNodeId, intermediateEdgeClosingNodes.OrderBy(n => n.Id).Last().Id);
                }
            }

            return polygons;
        }
    }

}

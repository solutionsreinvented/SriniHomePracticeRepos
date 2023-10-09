using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Geometry.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Geometry
{
    class Program
    {
        static void Main(string[] args)
        {
            Polygon polygon = new Polygon(PointsProvider.GetPoints().ToList());

            List<Node> closedPolygonNodes = polygon.Points.Union(polygon.ClosingLinePoints).ToList();

            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            IOSGeometryUI geometry = instance.Geometry;

            closedPolygonNodes.ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));

            List<Node> remainingNodes = new List<Node>(closedPolygonNodes);
            List<Node> remainingNodesPrevious = new List<Node>(closedPolygonNodes);


            double maxDim = 7.5;
            var plateId = 0;

            int runningNodeId = closedPolygonNodes.Count();

            HashSet<Plate> plates = new HashSet<Plate>();
            HashSet<Node> additionalNodes = new HashSet<Node>();

            double verificationAngle = 30.0;
            double minimumAngle = 15.0;

            do
            {
                bool infiniteLoopEncountered = false;

                for (int i = 0; i < remainingNodes.Count - 2; i++)
                {
                    Node nodeA = remainingNodes[i + 0];
                    Node nodeB = remainingNodes[i + 1];
                    Node nodeC = remainingNodes[i + 2];

                    Triangle triangle = new Triangle(nodeA, nodeB, nodeC);

                    if (triangle.IsATriangle && triangle.IsQualified(verificationAngle))
                    {
                        if (triangle.Sides.Last().Length <= maxDim)
                        {
                            plates.Add(new Plate(++plateId, nodeA, nodeB, nodeC));

                            remainingNodes.RemoveAt(i + 1);
                        }
                        else
                        {
                            Node intermediateNode = triangle.PerpendicularNodes[1];

                            intermediateNode.Id = ++runningNodeId;

                            additionalNodes.Add(intermediateNode);

                            plates.Add(new Plate(++plateId, nodeA, nodeB, intermediateNode));
                            plates.Add(new Plate(++plateId, intermediateNode, nodeB, nodeC));

                            remainingNodes.RemoveAt(i + 1);
                            remainingNodes.Insert(i + 1, intermediateNode);
                        }

                        break;
                    }
                    else
                    {
                        infiniteLoopEncountered = i == remainingNodes.Count - 3 && remainingNodesPrevious.Count == remainingNodes.Count && remainingNodes.All(n => remainingNodesPrevious.Contains(n));
                        continue;
                    }

                }

                if (infiniteLoopEncountered)
                {
                    verificationAngle -= 1.0;
                    runningNodeId = closedPolygonNodes.Count;
                    plateId = 0;
                    remainingNodes = new List<Node>(closedPolygonNodes);
                    remainingNodesPrevious = new List<Node>(closedPolygonNodes);
                    additionalNodes = new HashSet<Node>();
                    plates = new HashSet<Plate>();
                }
                else
                {
                    remainingNodesPrevious = remainingNodes;
                }

                additionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
                plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));

            } while (remainingNodes.Count > 2 && verificationAngle >= minimumAngle);

            if (verificationAngle >= minimumAngle)
            {
                additionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
                plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));
            }
            else
            {

            }


            Console.ReadLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Geometry.Base;
using ReInvented.Geometry.Interfaces;
using ReInvented.Geometry.Models;
using ReInvented.Geometry.Enums;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Services
{
    public class PlanarPolygonTriangulationServiceAlternative : PolygonTriangulationService, IPolygonTriangulationService
    {
        #region Parameterized Constructor

        public PlanarPolygonTriangulationServiceAlternative(TriangleSplitMethod triangleSplitMethod = TriangleSplitMethod.PerpendicularNode) : base(triangleSplitMethod)
        {

        }

        #endregion

        public override (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, int currentNodeId = 0, int currentPlateId = 0, double maximumDimension = 0)
        {
            throw new NotImplementedException();
        }


        public (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, double highestMinAngle = 30.0, double leastMinAngle = 15.0,
                                                                                            int currentNodeId = 0, int currentPlateId = 0, double maximumDimension = 0.0)
        {
            if (!(polygon is PlanarPolygon planarPolygon))
            {
                throw new ArgumentException($"{nameof(polygon)} shall of type {typeof(PlanarPolygon)}");
            }

            List<Node> polygonPoints = polygon.Points.ToList();

            if (currentNodeId == 0)
            {
                currentNodeId = polygonPoints.OrderBy(p => p.Id).Last().Id;
            }

            HashSet<Plate> plates = new HashSet<Plate>();
            HashSet<Node> additionalNodes = new HashSet<Node>();

            List<Node> remainingNodes = new List<Node>(polygonPoints);
            List<Node> remainingNodesPrevious = new List<Node>(polygonPoints);

            if (maximumDimension == 0.0)
            {
                maximumDimension = Node.LeastDistanceBetweenNodes(remainingNodes);
            }

            double verificationAngle = highestMinAngle;

            do
            {
                bool infiniteLoopEncountered = false;

                for (int i = 0; i < remainingNodes.Count - 2; i++)
                {
                    Node nodeA = remainingNodes[i + 0];
                    Node nodeB = remainingNodes[i + 1];
                    Node nodeC = remainingNodes[i + 2];
                    Node nodeN = i == 0 ? remainingNodes.Last() : remainingNodes[i - 1];

                    Triangle forwardTriangle = new Triangle(nodeA, nodeB, nodeC);
                    Triangle backwardTriangle = new Triangle(nodeN, nodeA, nodeB);

                    Triangle triangle = Triangle.GetSuitableTriangle(forwardTriangle, backwardTriangle, verificationAngle);

                    if (triangle != null)
                    {
                        if (triangle.Sides.Last().Length <= maximumDimension)
                        {
                            plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], triangle.Vertices.Last()));

                            remainingNodes.Remove(triangle.Vertices[1]);
                        }
                        else
                        {
                            Node intermediateNode;

                            if (TriangleSplitMethod == TriangleSplitMethod.PerpendicularNode)
                            {
                                intermediateNode = triangle.PerpendicularNodes[1];
                            }
                            else
                            {
                                intermediateNode = triangle.MidNodes.Last();
                            }

                            intermediateNode.Id = ++currentNodeId;

                            additionalNodes.Add(intermediateNode);

                            plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], intermediateNode));
                            plates.Add(new Plate(++currentPlateId, intermediateNode, triangle.Vertices[1], triangle.Vertices.Last()));

                            remainingNodes.Remove(triangle.Vertices[1]);

                            if (triangle == forwardTriangle)
                            {
                                remainingNodes.Insert(i + 1, intermediateNode);
                            }
                            else
                            {
                                remainingNodes.Insert(i + 0, intermediateNode);

                                /// Switches the last node as first node, when backward triangle is used
                                Node lastNode = remainingNodes.Last();
                                remainingNodes.Remove(lastNode);
                                remainingNodes.Insert(i + 0, lastNode);
                            }
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
                    List<Node> subPolygonPoints;

                    for (int i = 2; i < remainingNodes.Count - 1; i++)
                    {
                        subPolygonPoints = remainingNodes.Take(i + 1).ToList();
                        PlanarPolygon subPolygon = new PlanarPolygon(subPolygonPoints, true);

                        var (AdditionalNodes, Plates) = GenerateMesh(subPolygon, geometry, verificationAngle, verificationAngle, currentNodeId, currentPlateId);

                        if (AdditionalNodes != null && AdditionalNodes.Count > 0 && Plates != null && Plates.Count > 0)
                        {
                            AdditionalNodes.ToList().ForEach(an => additionalNodes.Add(an));
                            Plates.ToList().ForEach(p => plates.Add(p));
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }















                    verificationAngle -= 1.0;
                    currentNodeId = polygonPoints.Count;
                    currentPlateId = 0;
                    remainingNodes = new List<Node>(polygonPoints);
                    remainingNodesPrevious = new List<Node>(polygonPoints);

                    /// Testing only. Delete.
                    plates.ToList().ForEach(p => geometry.DeletePlate(p.Id));
                    additionalNodes.ToList().ForEach(n => geometry.DeleteNode(n.Id));
                    /// Till here.

                    additionalNodes = new HashSet<Node>();
                    plates = new HashSet<Plate>();

                }
                else
                {
                    remainingNodesPrevious = remainingNodes;
                }
                additionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
                plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));

            } while (remainingNodes.Count > 2 && verificationAngle >= leastMinAngle);

            return (additionalNodes, plates);
        }
    }
}

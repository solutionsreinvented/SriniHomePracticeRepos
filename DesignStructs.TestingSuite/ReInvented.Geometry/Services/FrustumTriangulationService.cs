using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Geometry.Base;
using ReInvented.Geometry.Enums;
using ReInvented.Geometry.Interfaces;
using ReInvented.Geometry.Models;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry
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
    }
    public class MeshCriteria
    {
        public double MaximumDimension { get; set; }
        public int PlateCurrentId { get; set; }
        public int NodeCurrentId { get; set; }
        public int HighestMinimumAngle { get; set; }

        public int LeastMinimumAngle { get; set; }
    }

    public class FrustumTriangulationService : PolygonTriangulationService, IPolygonTriangulationService
    {
        public FrustumTriangulationService(TriangleSplitMethod triangleSplitMethod = TriangleSplitMethod.PerpendicularNode) : base(triangleSplitMethod)
        {

        }

        public override (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(IPolygon polygon, IOSGeometryUI geometry, int currentNodeId = 0, int currentPlateId = 0, double maximumDimension = 0)
        {
            if (!(polygon is ConicalPolygon conicalPolygon))
            {
                throw new ArgumentException($"{nameof(polygon)} shall of type {typeof(ConicalPolygon)}");
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

            double verificationAngle = 30.0;
            double minimumAngle = 5.0;

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

                            /// TODO: This is directly resulting in infinite loop if the frustum is just a plane.
                            ///intermediateNode = AdjustIntermediateNodeCoordinatesForConicalSurface(conicalPolygon, intermediateNode);

                            intermediateNode.Id = ++currentNodeId;

                            additionalNodes.Add(intermediateNode);

                            plates.Add(new Plate(++currentPlateId, triangle.Vertices.First(), triangle.Vertices[1], intermediateNode));
                            plates.Add(new Plate(++currentPlateId, intermediateNode, triangle.Vertices[1], triangle.Vertices.Last()));

                            ///remainingNodes.RemoveAt(i + 1);
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

            } while (remainingNodes.Count > 2 && verificationAngle >= minimumAngle);

            return (additionalNodes, plates);
        }

        #region Private Helpers

        private static Node AdjustIntermediateNodeCoordinatesForConicalSurface(ConicalPolygon conicalPolygon, Node intermediateNode)
        {
            double angle = NodeHelpers.AngleIn360Degrees(conicalPolygon.StartCenter, intermediateNode);

            double intermediateRadius = conicalPolygon.StartRadius +
                                        ((conicalPolygon.EndRadius - conicalPolygon.StartRadius) / (conicalPolygon.EndYCoordinate - conicalPolygon.StartYCoordinate) * (intermediateNode.Y - conicalPolygon.StartCenter.Y));

            var updatedX = conicalPolygon.StartCenter.X + intermediateRadius * Math.Cos(angle.Radians());
            var updatedZ = conicalPolygon.StartCenter.Z + intermediateRadius * Math.Sin(angle.Radians());

            intermediateNode.X = updatedX;
            intermediateNode.Z = updatedZ;

            return intermediateNode;
        } 

        #endregion
    }
}

using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Geometry.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry
{
    public class MeshCriteria
    {
        public double MaximumDimension { get; set; }
        public int PlateCurrentId { get; set; }
        public int NodeCurrentId { get; set; }
    }

    public class PolygonMesher
    {
        /// TODO: This function seems to be messy and carefully review the usage of nodeId and plateId.
        ///       Issue is as follows:
        ///       1. Since this mesh generation involves iterative process, these Ids are considered independently.
        ///       2. However, these ids have to be in sync and continuation with the staad model ids.
        ///       3. MeshCriteria is defined within the function which needs to be separated and obtained from the user may be as part of the settings.
        ///       4. Check the possibility of refactoring this function in to multiple separate functions which follows SRP.
        public static (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) GenerateMesh(Polygon polygon, IOSGeometryUI geometry)
        {
            HashSet<Plate> plates = new HashSet<Plate>();
            HashSet<Node> additionalNodes = new HashSet<Node>();

            List<Node> remainingNodes = new List<Node>(polygon.ClosedPolygonPoints);
            List<Node> remainingNodesPrevious = new List<Node>(polygon.ClosedPolygonPoints);

            double maxDim = 7;
            var plateId = 0;

            int runningNodeId = polygon.ClosedPolygonPoints.Count();

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
                    Node nodeN = i == 0 ? remainingNodes.Last() : remainingNodes[i - 1];


                    Triangle forwardTriangle = new Triangle(nodeA, nodeB, nodeC);
                    Triangle backwardTriangle = new Triangle(nodeN, nodeA, nodeB);

                    Triangle triangle = Triangle.GetSuitableTriangle(forwardTriangle, backwardTriangle, verificationAngle);

                    if (triangle != null)
                    {
                        if (triangle.Sides.Last().Length <= maxDim)
                        {
                            plates.Add(new Plate(++plateId, triangle.Vertices.First(), triangle.Vertices[1], triangle.Vertices.Last()));

                            ///remainingNodes.RemoveAt(i + 1);
                            remainingNodes.Remove(triangle.Vertices[1]);
                        }
                        else
                        {
                            Node intermediateNode = triangle.PerpendicularNodes[1];

                            intermediateNode.Id = ++runningNodeId;

                            additionalNodes.Add(intermediateNode);

                            plates.Add(new Plate(++plateId, triangle.Vertices.First(), triangle.Vertices[1], intermediateNode));
                            plates.Add(new Plate(++plateId, intermediateNode, triangle.Vertices[1], triangle.Vertices.Last()));

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
                    runningNodeId = polygon.ClosedPolygonPoints.Count;
                    plateId = 0;
                    remainingNodes = new List<Node>(polygon.ClosedPolygonPoints);
                    remainingNodesPrevious = new List<Node>(polygon.ClosedPolygonPoints);

                    ///// Testing only. Delete.
                    //plates.ToList().ForEach(p => geometry.DeletePlate(p.Id));
                    //additionalNodes.ToList().ForEach(n => geometry.DeleteNode(n.Id));
                    ///// Till here.
                    
                    additionalNodes = new HashSet<Node>();
                    plates = new HashSet<Plate>();

                }
                else
                {
                    remainingNodesPrevious = remainingNodes;
                }
                //additionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
                //plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));

            } while (remainingNodes.Count > 2 && verificationAngle >= minimumAngle);

            return (additionalNodes, plates);
        }
    }
}

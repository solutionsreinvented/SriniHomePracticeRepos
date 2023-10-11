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
            double aspectRatio = 4;

            ///List<Node> polygonPoints = RegularPolygonPointsProvider.GetPoints();
            ///List<Node> polygonPoints = OpenAnnularRingPointsProvider.GetPoints(10.0, 8.0, 30, 30, 0.0, 0);
            ///List<Node> polygonPoints = ClosedAnnularRingPointsProvider.GetPoints(10.0, 3.0, 30, 25, 0.0, 0);
            ///List<Node> polygonPoints = ClosedAnnularRingPointsProvider.GetPoints(10.0, 7.0, 60, 48, 0.0, 0);
            ///List<Node> polygonPoints = SolidCircularPlatePointsProvider.GetPoints(10.0, 60, 0.0, 0, 0);

            var polygonPoints = MountingPlateNodesProvider.GetPoints(6, 34, 5);

            Polygon polygon = new Polygon(polygonPoints);

            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            IOSGeometryUI geometry = instance.Geometry;

            ///List<Node> closedPolygonPoints = polygon.Points.Union(polygon.GetClosingLinePoints()).ToList();
            List<Node> closedPolygonPoints = polygon.Points.ToList();

            closedPolygonPoints.ToHashSet().ToList().ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));

            ///Triangulation triangulation = new Triangulation(TriangleSplitMethod.PerpendicularNode);
            Triangulation triangulation = new Triangulation(TriangleSplitMethod.MidNode);


            (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) = triangulation.GenerateMesh(closedPolygonPoints, geometry, closedPolygonPoints.Count(), 0, aspectRatio * Node.LeastDistanceBetweenNodes(closedPolygonPoints));

            if (AdditionalNodes != null && AdditionalNodes.Count > 1)
            {
                AdditionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
            }
            if (Plates != null && Plates.Count > 1)
            {
                Plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));
            }

            Console.ReadLine();
        }
    }
}

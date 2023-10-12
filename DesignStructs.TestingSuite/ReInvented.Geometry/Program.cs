using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Geometry.Enums;
using ReInvented.Geometry.Interfaces;
using ReInvented.Geometry.Models;
using ReInvented.Geometry.Services;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Geometry
{
    class Program
    {

        #region Usages

        static (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) UsingPlanarPolygonTriangulationService(IOSGeometryUI geometry, double aspectRatio = 2.0)
        {
            List<Node> polygonPoints = RegularPolygonPointsProvider.GetPoints();
            PlanarPolygon polygon = new PlanarPolygon(polygonPoints);
            IReadOnlyList<Node> closedPolygonPoints = polygon.Points;

            closedPolygonPoints.ToHashSet().ToList().ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));

            IPolygonTriangulationService triangulationService = new PlanarPolygonTriangulationService(TriangleSplitMethod.MidNode);
            return triangulationService.GenerateMesh(polygon, geometry, 0, 0, aspectRatio * Node.LeastDistanceBetweenNodes(closedPolygonPoints.ToList()));
        }

        static (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) UsingConeFrustumTriangulationService(IOSGeometryUI geometry, double aspectRatio = 2.0)
        {
            Node origin = new Node(0.0, 0.0, 0.0);
            List<Node> startCircleNodes = Circle.GenerateNodes(origin, 12.0, 48);
            List<Node> endCircleNodes = Circle.GenerateNodes(origin, 15.0, 64, 0, startCircleNodes.OrderBy(n => n.Id).Last().Id);

            ConeFrustum coneFrustum = new ConeFrustum(startCircleNodes, endCircleNodes, edgesAreOpen: false, formation: MeshFormation.Clockwise);
            IPolygon polygon = coneFrustum.GetPolygon();

            polygon.Points.ToHashSet().ToList().ForEach(p => geometry.CreateNode(p.Id, p.X, p.Y, p.Z));

            IPolygonTriangulationService triangulationService = new FrustumTriangulationService(TriangleSplitMethod.MidNode);
            return triangulationService.GenerateMesh(polygon, geometry, 0, 0, aspectRatio * Node.LeastDistanceBetweenNodes(polygon.Points));
        }

        #endregion


        static void Main(string[] args)
        {


            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            IOSGeometryUI geometry = instance.Geometry;

            double aspectRatio = 3;

            ///var (AdditionalNodes, Plates) = UsingPlanarPolygonTriangulationService(geometry, aspectRatio);
            var (AdditionalNodes, Plates) = UsingConeFrustumTriangulationService(geometry, aspectRatio);

            //if (AdditionalNodes != null && AdditionalNodes.Count > 1)
            //{
            //    AdditionalNodes.ToList().ForEach(an => geometry.CreateNode(an.Id, an.X, an.Y, an.Z));
            //}
            //if (Plates != null && Plates.Count > 1)
            //{
            //    Plates.ToList().ForEach(p => geometry.CreatePlate(p.Id, p.A.Id, p.B.Id, p.C.Id, p.D.Id));
            //}

            Console.ReadLine();
        }

        #region Tests

        private static void TestMethod(Node origin)
        {
            List<Node> points = Circle.GenerateNodes(origin, 10, 25, 0.5);

            double x = points.Average(p => p.X);
            double y = points.Average(p => p.Y);
            double z = points.Average(p => p.Z);

            double radius = Math.Sqrt((points.First().X - x).Squared() + (points.First().Y - y).Squared() + (points.First().Z - z).Squared());
        }

        #endregion
    }
}

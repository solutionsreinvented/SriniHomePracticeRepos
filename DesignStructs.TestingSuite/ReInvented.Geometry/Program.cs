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
            double aspectRatio = 2.5;

            //List<Node> polygonPoints = OpenAnnularRingPointsProvider.GetPoints(10.0, 10.0, 30, 30, 0.0, -6.50);
            var polygonPoints = RegularPointsProvider.GetPoints();


            Polygon polygon = new Polygon(polygonPoints, closingLinePointsAdded: false);

            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            IOSGeometryUI geometry = instance.Geometry;

            polygon.ClosedPolygonPoints.ToHashSet().ToList().ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));

            (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) = Triangulation.GenerateMesh(polygon, geometry, polygon.ClosedPolygonPoints.Count(), 0, aspectRatio * Node.LeastDistanceBetweenNodes(polygon.ClosedPolygonPoints.ToList()));

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

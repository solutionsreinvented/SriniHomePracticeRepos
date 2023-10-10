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
            List<Node> annularRingPoints = OpenAnnularRingPointsProvider.GetPoints(10.0, 8.0, 30, 30);

            Polygon polygon = new Polygon(annularRingPoints);

            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            IOSGeometryUI geometry = instance.Geometry;

            polygon.ClosedPolygonPoints.ToHashSet().ToList().ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z));

            (HashSet<Node> AdditionalNodes, HashSet<Plate> Plates) = Triangulation.GenerateMesh(polygon, geometry);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Geometry.Models;

namespace ReInvented.Geometry
{
    class Program
    {
        static void Main(string[] args)
        {
            Polygon polygon = new Polygon(PointsProvider.GetPoints().ToList());

            List<Point3D> closedPolygonPoints = polygon.Points.Union(polygon.ClosingLinePoints).ToList();

            List<Point3D> remainingPoints = closedPolygonPoints;

            do
            {
                for (int i = 0; i < remainingPoints.Count - 2; i++)
                {
                    var p1 = remainingPoints[i + 0];
                    var p2 = remainingPoints[i + 1];
                    var p3 = remainingPoints[i + 2];

                    Triangle triangle = new Triangle(p1, p2, p3);

                    if (triangle.IsATriangle)
                    {

                    }
                }


            } while (remainingPoints.Count > 0);


            Arc arc = new Arc(new Point3D(6, 0, 1), new Point3D(5.330127019, 0, 3.5));
            arc.GetPropertiesInfo().ForEach(pi => Console.WriteLine(pi));

            Console.ReadLine();
        }
    }
}

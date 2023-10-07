using System;
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

            polygon.GenerateClosingPoints();

            Arc arc = new Arc(new Point3D(6, 0, 1), new Point3D(5.330127019, 0, 3.5));
            arc.GetPropertiesInfo().ForEach(pi => Console.WriteLine(pi));

            Console.ReadLine();
        }
    }
}

using System;
using System.Linq;
using System.Windows.Media.Media3D;

namespace ReInvented.Geometry.Models
{
    public class Triangle
    {
        public Triangle(Point3D p1, Point3D p2, Point3D p3)
        {
            Vertices = new Point3DCollection() { p1, p2, p3 };

            CalculateProperties();
        }


        public Point3DCollection Vertices { get; private set; }

        public Vector3DCollection Sides { get; private set; }

        public double[] Angles { get; private set; }

        public bool IsATriangle => !Angles.Any(a => a == 180.0 || a == double.NaN);

        #region Private Helpers

        private void CalculateProperties()
        {
            Sides = new Vector3DCollection
            {
                new Vector3D(Vertices[1].X - Vertices[0].X, Vertices[1].Y - Vertices[0].Y, Vertices[1].Z - Vertices[0].Z),
                new Vector3D(Vertices[2].X - Vertices[1].X, Vertices[2].Y - Vertices[1].Y, Vertices[2].Z - Vertices[1].Z),
                new Vector3D(Vertices[0].X - Vertices[2].X, Vertices[0].Y - Vertices[2].Y, Vertices[0].Z - Vertices[1].Z)
            };

            Angles = new double[]
            {
                Math.Round(Vector3D.AngleBetween(Sides[0], (-1) * Sides[2]), 1),
                Math.Round(Vector3D.AngleBetween(Sides[1], (-1) * Sides[0]), 1),
                Math.Round(Vector3D.AngleBetween(Sides[2], (-1) * Sides[1]), 1)
            };

        }

        #endregion
    }
}

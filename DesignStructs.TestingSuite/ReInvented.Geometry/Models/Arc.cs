using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

using ReInvented.Shared;

namespace ReInvented.Geometry.Models
{

    public class Arc
    {
        private readonly Point3D _arcStart, _arcEnd;

        public Arc(Point3D arcStart, Point3D arcEnd)
        {
            _arcStart = arcStart;
            _arcEnd = arcEnd;

            CalculateCenter();
            CalculateRadius();
            CalculateIncludedAngle();
        }

        public Point3D Center { get; private set; }

        public double IncludedAngle { get; private set; }

        public double Radius { get; private set; }

        // Property to calculate the arc length
        public double ArcLength => (IncludedAngle / 360.0) * (2 * Math.PI * Radius);

        public List<string> GetPropertiesInfo()
        {
            return new List<string>()
            {
                $"Center ({Center.X:N3}, {Center.Y:N3}, {Center.Z:N3})",
                $"Included angle: {IncludedAngle:N2}",
                $"Radius: {Radius:N2}",
                $"Arc length: {ArcLength:N5}"
            };
        }


        private void CalculateCenter()
        {
            double centerX = (_arcStart.X + _arcEnd.X) / 2;
            double centerY = (_arcStart.Y + _arcEnd.Y) / 2;
            double centerZ = (_arcStart.Z + _arcEnd.Z) / 2;

            Center = new Point3D(centerX, centerY, centerZ);
        }

        private void CalculateIncludedAngle()
        {
            Vector3D vector1 = _arcStart - Center;
            Vector3D vector2 = _arcEnd - Center;

            // Calculate the dot product and the cross product magnitude
            double dotProduct = Vector3D.DotProduct(vector1, vector2);
            double crossProductMagnitude = vector1.Length * vector2.Length;

            // Calculate the included angle in radians
            double angleInRadians = Math.Acos(dotProduct / crossProductMagnitude);

            // Convert the angle from radians to degrees
            IncludedAngle = RadiansToDegrees(angleInRadians);

            // Ensure the angle is in the range [0, 360)
            if (IncludedAngle < 0) IncludedAngle += 360.0;
        }

        private void CalculateRadius()
        {
            // Calculate the radius as the distance from the center to either of the points
            ///Radius = Math.Sqrt(Math.Pow(Center.X - _arcStart.X, 2) + Math.Pow(Center.Y - _arcStart.Y, 2) + Math.Pow(Center.Z - _arcStart.Z, 2));
            double length = Math.Sqrt((_arcEnd.X - _arcStart.X).Squared() + (_arcEnd.Z - _arcStart.Z).Squared());
            double d = Math.Sqrt(((_arcStart.X + _arcEnd.X) / 2.0).Squared() + ((_arcStart.Z + _arcEnd.Z) / 2.0).Squared());
            Radius = d / 2;
        }

        private double RadiansToDegrees(double radians)
        {
            return radians * (180.0 / Math.PI);
        }


    }
}

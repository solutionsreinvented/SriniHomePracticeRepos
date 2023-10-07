using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Shared;

namespace ReInvented.Geometry.Models
{
    public class PointsProvider
    {
        public static HashSet<Point3D> GetPoints()
        {
            return new HashSet<Point3D>()
            {
                new Point3D(0, 0, 0), new Point3D(0, 5, 0), new Point3D(0, 10, 0), new Point3D(0, 15, 0), new Point3D(0, 20, 0), new Point3D(5, 20, 0),
                new Point3D(10, 20, 0), new Point3D(20, 20, 0)///, new Point3D(20, 15, 0), new Point3D(20, 10, 0)
            };
        }
    }
    public class Polygon
    {
        public Polygon(List<Point3D> points)
        {
            /// TODO: Implement verification for nulls
            Points = points;
            GenerateVector3DCollection();
        }

        public IReadOnlyList<Point3D> Points { get; private set; }

        public List<Point3D> ClosingLinePoints { get; private set; }

        public Vector3DCollection Vectors { get; private set; }

        public double TotalIncludeAngle => CalculateTotalIncludedAngle();

        public void GenerateClosingPoints(double maxDimension = 5)
        {
            ClosingLinePoints = new List<Point3D>();

            double lineLength = (Points.Last() - Points.First()).Length;
            Vector3D lineVector = Vectors.Last();
            lineVector.Normalize();

            int nNewNodes = (int)(lineLength / maxDimension).Floor(1);
            double adjustedDimension = lineLength / nNewNodes;

            for (int i = 1; i < nNewNodes; i++)
            {
                var newVector = Vector3D.Multiply(i * adjustedDimension, lineVector);

                ClosingLinePoints.Add(new Point3D(Points.Last().X + newVector.X, Points.Last().Y + newVector.Y, Points.Last().Z + newVector.Z));
            }
        }

        #region Private Helpers

        private void GenerateVector3DCollection()
        {
            Vectors = new Vector3DCollection();

            for (int i = 0; i < Points.Count(); i++)
            {
                Point3D startPoint = i == Points.Count() - 1 ? Points.Last() : Points[i];
                Point3D endPoint = i == Points.Count() - 1 ? Points.First() : Points[i + 1];

                Vectors.Add(new Vector3D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z));
            }
        }

        private double CalculateTotalIncludedAngle()
        {
            if (Vectors == null || Vectors.Count != Points.Count)
            {
                GenerateVector3DCollection();
            }

            double totalAngle = 0.0;
            double angle;

            for (int i = 0; i < Vectors.Count(); i++)
            {
                Vector3D forwardVector = i == Vectors.Count() - 1 ? Vectors.First() : Vectors[i + 1];
                Vector3D backwardVector = i == Vectors.Count() - 1 ? Vectors.Last() : Vectors[i];

                angle = Math.Round(Vector3D.AngleBetween(forwardVector, (-1) * backwardVector), 1);

                if (angle != 180.0)
                {
                    totalAngle += angle;
                }
            }

            return Math.Round(totalAngle, 1);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Base
{
    public abstract class Polygon
    {
        #region Parameterized Constructor

        public Polygon(List<Node> points)
        {
            /// TODO: Implement verification for nulls
            Points = points;
            GenerateVector3DCollection();
        }

        #endregion

        #region Public Properties

        public List<Node> Points { get; private set; }

        public double TotalIncludeAngle => CalculateTotalIncludedAngle();

        public Vector3DCollection Vectors { get; private set; }

        #endregion

        #region Private Helpers

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

        private void GenerateVector3DCollection()
        {
            Vectors = new Vector3DCollection();

            for (int i = 0; i < Points.Count(); i++)
            {
                Node startPoint = i == Points.Count() - 1 ? Points.Last() : Points[i];
                Node endPoint = i == Points.Count() - 1 ? Points.First() : Points[i + 1];

                Vectors.Add(new Vector3D(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z));
            }
        }

        #endregion
    }
}

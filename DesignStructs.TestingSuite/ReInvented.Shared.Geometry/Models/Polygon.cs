using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Shared.Geometry.Interfaces;
using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.Shared.Geometry.Models
{
    public class Polygon : IPolygon
    {

        #region Public Properties

        public List<Node> Vertices { get; private set; }

        public double TotalIncludeAngle => CalculateTotalIncludedAngle();

        public Vector3DCollection Sides { get; private set; }

        #endregion

        #region Private Helpers

        private double CalculateTotalIncludedAngle()
        {
            if (Sides == null || Sides.Count != Vertices.Count)
            {
                GenerateVector3DCollection();
            }

            double totalAngle = 0.0;
            double angle;

            for (int i = 0; i < Sides.Count(); i++)
            {
                Vector3D forward = i == Sides.Count() - 1 ? Sides.First() : Sides[i + 1];
                Vector3D backward = i == Sides.Count() - 1 ? Sides.Last() : Sides[i];

                angle = Math.Round(Vector3D.AngleBetween(forward, (-1) * backward), 1);

                if (angle != 180.0)
                {
                    totalAngle += angle;
                }
            }

            return Math.Round(totalAngle, 1);
        }

        private void GenerateVector3DCollection()
        {
            Sides = new Vector3DCollection();

            for (int i = 0; i < Vertices.Count(); i++)
            {
                Node sPoint = i == Vertices.Count() - 1 ? Vertices.Last() : Vertices[i];
                Node ePoint = i == Vertices.Count() - 1 ? Vertices.First() : Vertices[i + 1];

                Sides.Add(new Vector3D(ePoint.X - sPoint.X, ePoint.Y - sPoint.Y, ePoint.Z - sPoint.Z));
            }
        }

        #endregion
    }
}

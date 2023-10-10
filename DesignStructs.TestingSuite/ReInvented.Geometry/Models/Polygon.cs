﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class Polygon
    {
        public Polygon(List<Node> points, bool closingLinePointsAdded = true)
        {
            /// TODO: Implement verification for nulls
            Points = points;
            ClosingLinePointsAdded = closingLinePointsAdded;
            GenerateVector3DCollection();
        }

        public IReadOnlyList<Node> Points { get; private set; }

        public double TotalIncludeAngle => CalculateTotalIncludedAngle();

        public List<Node> ClosingLinePoints => GenerateClosingLinePoints();

        public IReadOnlyList<Node> ClosedPolygonPoints =>  ClosingLinePointsAdded ? Points : Points.Union(ClosingLinePoints).ToList();

        public bool ClosingLinePointsAdded { get; private set; }

        public Vector3DCollection Vectors { get; private set; }

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

        private List<Node> GenerateClosingLinePoints()
        {
            var closingLinePoints = new List<Node>();
            var nodeId = Points.Count();

            double lineLength = (Points.Last().ToVector - Points.First().ToVector).Length;
            Vector3D lineVector = Vectors.Last();
            lineVector.Normalize();

            double maximumDimension = Node.LeastDistanceBetweenNodes(Points.ToList());

            int nNewNodes = (int)(lineLength / maximumDimension).Floor(1);
            double adjustedDimension = lineLength / nNewNodes;

            for (int i = 1; i < nNewNodes; i++)
            {
                Vector3D newVector = Vector3D.Multiply(i * adjustedDimension, lineVector);
                nodeId += 1;
                closingLinePoints.Add(new Node(nodeId, Points.Last().X + newVector.X, Points.Last().Y + newVector.Y, Points.Last().Z + newVector.Z));
            }

            return closingLinePoints;
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

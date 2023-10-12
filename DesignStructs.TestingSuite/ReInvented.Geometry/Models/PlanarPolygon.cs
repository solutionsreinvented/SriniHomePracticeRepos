using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Geometry.Base;
using ReInvented.Geometry.Interfaces;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public sealed class PlanarPolygon : Polygon, IPolygon
    {
        #region Parameterized Constructor

        public PlanarPolygon(List<Node> points, bool generateClosingLinePoints = true) : base(points)
        {
            if (generateClosingLinePoints)
            {
                Points.AddRange(GetClosingLinePoints());
            }
        }

        #endregion

        #region Public Properties

        public bool GenerateClosingLinePoints { get; private set; } 

        #endregion

        #region Private Functions

        private List<Node> GetClosingLinePoints(int currentNodeId = 0)
        {
            List<Node> closingLinePoints = new List<Node>();

            if (Points == null || Points.Count <= 3)
            {
                return null;
            }

            if (currentNodeId == 0)
            {
                currentNodeId = Points.OrderBy(p => p.Id).Last().Id;
            }

            double lineLength = (Points.Last().ToVector - Points.First().ToVector).Length;
            Vector3D lineVector = Vectors.Last();
            lineVector.Normalize();

            double maximumDimension = Node.LeastDistanceBetweenNodes(Points.ToList());

            int nNewNodes = (int)(lineLength / maximumDimension).Floor(1);
            double adjustedDimension = lineLength / nNewNodes;

            for (int i = 1; i < nNewNodes; i++)
            {
                Vector3D newVector = Vector3D.Multiply(i * adjustedDimension, lineVector);
                currentNodeId += 1;
                closingLinePoints.Add(new Node(currentNodeId, Points.Last().X + newVector.X, Points.Last().Y + newVector.Y, Points.Last().Z + newVector.Z));
            }

            return closingLinePoints;
        }

        #endregion
    }

}

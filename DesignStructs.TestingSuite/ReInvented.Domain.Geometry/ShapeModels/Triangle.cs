using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Models
{
    public enum TriangleFormation
    {
        Clockwise,
        CounterClockwise,
        Collinear
    }
    public enum MeshFormation
    {
        Clockwise,
        CounterClockwise,
    }

    public class Triangle
    {
        public Triangle(Node vertexA, Node vertexB, Node vertexC)
        {
            Vertices = new HashSet<Node>() { vertexA, vertexB, vertexC }.ToList();

            CalculateProperties();
        }


        public List<Node> Vertices { get; private set; }

        public List<Node> PerpendicularNodes { get; private set; }

        public List<Node> MidNodes { get; private set; }

        public Vector3DCollection Sides { get; private set; }

        public double Area { get; private set; }

        public double[] Angles { get; private set; }

        public double MinimumAngle { get; private set; } = 15.0;

        public double MaximumAngle { get; private set; } = 30.0;

        public bool IsATriangle => !Angles.Any(a => a == 180.0 || a == double.NaN);

        public TriangleFormation TriangleFormation => GetTriangleFormation();

        public bool IsQualified(double minimumAngle) => Angles.All(c => c >= minimumAngle);

        public static Triangle GetSuitableTriangle(Triangle forwardTriangle, Triangle backwardTriangle, double minimumQualifyingAngle = 30.0)
        {
            Triangle suitableTriangle = null;

            if (forwardTriangle.IsATriangle && forwardTriangle.IsQualified(minimumQualifyingAngle))
            {
                if (backwardTriangle.IsATriangle && backwardTriangle.IsQualified(minimumQualifyingAngle))
                {
                    suitableTriangle = forwardTriangle.Angles.Max() < backwardTriangle.Angles.Max() ? forwardTriangle : backwardTriangle;
                }
                else
                {
                    suitableTriangle = forwardTriangle;
                }
            }
            else if (backwardTriangle.IsATriangle && backwardTriangle.IsQualified(minimumQualifyingAngle))
            {
                suitableTriangle = backwardTriangle;
            }

            return suitableTriangle;
        }

        #region Private Helpers

        private void CalculateProperties()
        {
            ///TODO: In some cases the vertices are only two. Not sure why this is happening but at this moment
            ///      due to the lack of time this is not explored. So fix this as soon as the other implementations
            ///      are completed. For the time being the control is returned. 
            if (Vertices.Count != 3)
            {
                return;
            }

            Sides = new Vector3DCollection
            {
                new Vector3D(Vertices[1].X - Vertices[0].X, Vertices[1].Y - Vertices[0].Y, Vertices[1].Z - Vertices[0].Z),
                new Vector3D(Vertices[2].X - Vertices[1].X, Vertices[2].Y - Vertices[1].Y, Vertices[2].Z - Vertices[1].Z),
                new Vector3D(Vertices[0].X - Vertices[2].X, Vertices[0].Y - Vertices[2].Y, Vertices[0].Z - Vertices[2].Z)
            };

            Angles = new double[]
            {
                Math.Round(Vector3D.AngleBetween(Sides[0], (-1) * Sides[2]), 1),
                Math.Round(Vector3D.AngleBetween(Sides[1], (-1) * Sides[0]), 1),
                Math.Round(Vector3D.AngleBetween(Sides[2], (-1) * Sides[1]), 1)
            };

            Area = 0.50 * Sides.First().Length * Sides.Last().Length * Math.Sin(Angles.First().Radians());

            PerpendicularNodes = new List<Node>()
            {
                /// Nodes on the triangle side where the perpendicular bisector is created from the vertex. 
                /// Node at index 0 indicates the node created on the side AB and continues sequentially to side AC
                Node.CreateNewNodeByDistance(Vertices.First(), Vertices[1], Sides.Last().Length * Math.Cos(Angles.First().Radians())),
                Node.CreateNewNodeByDistance(Vertices.First(), Vertices.Last(), Sides.First().Length * Math.Cos(Angles.First().Radians())),
                Node.CreateNewNodeByDistance(Vertices.Last(), Vertices[1], Sides.Last().Length * Math.Cos(Angles.Last().Radians()))
            };
            MidNodes = new List<Node>()
            {
                /// Nodes on the triangle side where the mid node is created from the vertex. 
                /// Node at index 0 indicates the node created on the side AB and continues sequentially to side AC
                Node.CreateNewNodeByProportion(Vertices.First(), Vertices[1], 0.50),
                Node.CreateNewNodeByProportion(Vertices[1], Vertices.Last(), 0.50),
                Node.CreateNewNodeByProportion(Vertices.First(), Vertices.Last(), 0.50)
            };
        }

        #endregion

        #region Private Static Functions

        private TriangleFormation GetTriangleFormation()
        {
            Vector3D crossProduct = Vector3D.CrossProduct(Sides[0], Sides[1]);
            TriangleFormation direction;
            if (crossProduct.Z > 0)
            {
                direction = TriangleFormation.CounterClockwise;
            }
            else if (crossProduct.Z < 0)
            {
                direction = TriangleFormation.Clockwise;
            }
            else
            {
                direction = TriangleFormation.Collinear;
            }

            return direction;
        } 

        #endregion

    }
}

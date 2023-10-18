using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Geometry.Models
{
    public class Circle
    {
        #region Parameterized Constructors

        public Circle(double radius)
        {
            Radius = radius;
            Center = new Node(0.0, 0.0, 0.0);
        }

        public Circle(Node center, double radius) : this(radius)
        {
            Center = center;
        }

        #endregion

        #region Public Properties

        public Node Center { get; private set; }

        public double Radius { get; private set; } 

        #endregion

        public List<Node> GenerateNodes(int nPoints, double elevationFromCenter = 0.0, int currentNodeId = 0)
        {
            return GenerateNodes(Center, Radius, nPoints, elevationFromCenter, currentNodeId);
        }

        public static List<Node> GenerateNodes(Node center, double radius, int nPoints, double elevationFromCenter = 0.0, int currentNodeId = 0)
        {
            List<Node> nodes = new List<Node>();

            double dTheta = 360.0 / nPoints;

            for (int i = 0; i < nPoints; i++)
            {
                currentNodeId += 1;
                double xCoordinate = center.X + radius * Math.Cos((i * dTheta).Radians());
                double yCoordinate = center.Y + elevationFromCenter;
                double zCoordinate = center.Z + radius * Math.Sin((i * dTheta).Radians());

                nodes.Add(new Node(currentNodeId, xCoordinate, yCoordinate, zCoordinate));
            }

            return nodes;
        }

        public static Node GetCenter(List<Node> nodesOnCircle, int roundRigits = 3)
        {
            Node center = new Node(Math.Round(nodesOnCircle.Average(n => n.X), roundRigits), Math.Round(nodesOnCircle.Average(n => n.Y), roundRigits), Math.Round(nodesOnCircle.Average(n => n.Z), roundRigits));
            return center;
        }
    }
}

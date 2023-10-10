using System;
using System.Collections.Generic;

using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Geometry.Models
{
    public class CirclePointsGenerator
    {
        public static List<Node> GetPoints(double radius, int points, double yCoordinate = 0.0, int currentNodeId = 0)
        {
            List<Node> nodes = new List<Node>();

            double dTheta = 360.0 / points;

            for (int i = 0; i < points; i++)
            {
                currentNodeId += 1;
                double xCoordinate = radius * Math.Cos((i * dTheta).ToRadians());
                double zCoordinate = radius * Math.Sin((i * dTheta).ToRadians());

                nodes.Add(new Node(currentNodeId, xCoordinate, yCoordinate, zCoordinate));
            }

            return nodes;
        }
    }

}

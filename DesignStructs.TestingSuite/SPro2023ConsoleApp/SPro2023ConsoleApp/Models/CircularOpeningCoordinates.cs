using System;
using System.Collections.Generic;
using ReInvented.Shared;
using ReInvented.StaadPro.Interactivity.Entities;

namespace SPro2023ConsoleApp.Models
{
    public class CircularOpeningCoordinates
    {
        public static List<Node> CalculateCoordinates(double rShell, double height, double yOpeningCenter, double rOpening, int nPoints, double alpha, double phi, int startId, bool inRangePointsOnly)
        {
            if (startId <= 0) { startId = 1; }

            List<Node> points = new List<Node>();

            double angleIncrement = 2 * Math.PI / nPoints;
            double phiDash = 90.0 - phi;

            for (int i = 0; i < nPoints; i++)
            {
                double iTheta = i * angleIncrement;
                double iX = rOpening * Math.Cos(iTheta); /// Horizontal distance to the ith point on the opening periphery.
                double rProj = iX / Math.Cos(phiDash.Radians());
                double sideB = rShell - rProj;
                Triangle triangle = new Triangle(rShell, sideB, phi);

                double iBeta = (alpha + triangle.AngleC).Radians(); // In Radians

                double y = rOpening * Math.Sin(iTheta);

                double yCoordinate = yOpeningCenter - y;
                double xCoordinate = rShell * Math.Cos(iBeta);
                double zCoordinate = rShell * Math.Sin(iBeta);

                //if (!inRangePointsOnly || ((yOpeningCenter - yCoordinate) >= 0 && yCoordinate <= Height))
                //{
                points.Add(new Node(startId + i, xCoordinate, yCoordinate, zCoordinate));
                //}
            }

            return points;
        }

        public static List<Node> CalculateCoordinates(double rShell, double H, double rOpening, double yOpeningCenter, int nPoints, double alpha, int startId, bool inRangePointsOnly)
        {
            return CalculateCoordinates(rShell, H, rOpening, yOpeningCenter, nPoints, alpha, alpha, startId, inRangePointsOnly);
        }

            public static List<Node> CalculateCoordinates(double rShell, double H, double rOpening, double yOpeningCenter, int nPoints, double alpha, int startId)
        {
            return CalculateCoordinates(rShell, H, rOpening, yOpeningCenter, nPoints, alpha, alpha, startId, false);
            //if (startId <= 0) { startId = 1; }

            //List<Node> points = new List<Node>();

            //// Calculate the angle between each point on the circumference
            //double angleIncrement = 2 * Math.PI / nPoints;

            //// Calculate the coordinates of each point
            //for (int i = 0; i < nPoints; i++)
            //{
            //    double iTheta = i * angleIncrement;
            //    double iBeta = Math.Acos(rOpening * Math.Cos(iTheta) / rShell) - alpha.Radians(); // In Radians

            //    double y = rOpening * Math.Sin(iTheta);

            //    double yCoordinate = yOpeningCenter - y;
            //    double xCoordinate = rShell * Math.Sin(iBeta);
            //    double zCoordinate = rShell * Math.Cos(iBeta);

            //    //if (excludeOutOfRangeNodes && (yOpeningCenter - yCoordinate) >= 0 && yCoordinate <= H)
            //    //{
            //        points.Add(new Node(startId + i, xCoordinate, yCoordinate, zCoordinate));
            //    //}
            //}

            //return points;
        }
        public static List<Node> CalculateCoordinates(double rShell, double H, double rOpening, double yOpeningCenter, int nPoints, double alpha)
        {
            return CalculateCoordinates(rShell, H, rOpening, yOpeningCenter, nPoints, alpha, 1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services;

namespace SPro2023ConsoleApp.Services
{
    public class PlateSurfaceAreaService
    {
        public static double Calculate(StaadModelWrapper wrapper, HashSet<int> platesList)
        {
            List<int> platesIds = platesList.ToList();
            double totalSurfaceArea = 0.0;

            for (int i = 0; i < platesIds.Count; i++)
            {
                IEnumerable<Node> plateIncidence = StaadGeometryServices.GetPlateIncidence(wrapper, platesIds[i]);
                totalSurfaceArea += Calculate(plateIncidence.ToList());
            }

            return totalSurfaceArea;
        }

        public static double Calculate(List<Node> plateVertices)
        {
            if (plateVertices.Count < 3)
            {
                throw new ArgumentException($"Minimum number of nodes in {nameof(plateVertices)} shall be 3");
            }
            if (plateVertices.Count == 3)
            {
                return Calculate(plateVertices[0], plateVertices[1], plateVertices[2]);
            }

            double area1 = Calculate(plateVertices[0], plateVertices[1], plateVertices[2]);
            double area2 = Calculate(plateVertices[0], plateVertices[2], plateVertices[3]);

            double totalArea = area1 + area2;

            return totalArea;
        }

        public static double Calculate(Node a, Node b, Node c)
        {
            if (a == null || b == null || c == null)
            {
                throw new ArgumentException($"One or more of the provided vertices is/are null. Please provide valid vertices data.");
            }

            Vector3D v1 = b.ToVector - a.ToVector;
            Vector3D v2 = c.ToVector - a.ToVector;

            Vector3D normal1 = Vector3D.CrossProduct(v1, v2);

            double area = 0.5 * normal1.Length;

            return area;
        }
    }
}

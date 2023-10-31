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
                plateVertices.Add(plateVertices[0]);
            }

            Vector3D v1 = plateVertices[1].ToVector - plateVertices[0].ToVector;
            Vector3D v2 = plateVertices[2].ToVector - plateVertices[0].ToVector;
            Vector3D v3 = plateVertices[3].ToVector - plateVertices[0].ToVector;

            Vector3D normal1 = Vector3D.CrossProduct(v1, v2);
            Vector3D normal2 = Vector3D.CrossProduct(v1, v3);

            double area1 = 0.5 * normal1.Length;
            double area2 = 0.5 * normal2.Length;

            double totalArea = area1 + area2;

            return totalArea;

        }
    }
}

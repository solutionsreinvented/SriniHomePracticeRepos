using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;

namespace ReInvented.Domain.Reporting.Services
{
    public class PlateSurfaceAreaService
    {

        public static double Calculate(OSGeometryUI geometry, HashSet<int> platesList)
        {
            List<int> platesIds = platesList.ToList();
            double totalSurfaceArea = 0.0;

            for (int i = 0; i < platesIds.Count; i++)
            {
                IEnumerable<Node> plateIncidence = geometry.GetPlateIncidence(platesIds[i]);
                totalSurfaceArea += Calculate(plateIncidence.ToList());
            }

            return totalSurfaceArea;
        }

        public static double Calculate(Plate plate)
        {
            if (plate == null || plate.A == null || plate.B == null || plate.C == null || plate.D == null)
            {
                throw new ArgumentException($"{nameof(plate)} contains invalid data.");
            }

            return plate.A == plate.D ? Calculate(plate.A, plate.B, plate.C) :
                                        Calculate(plate.A, plate.B, plate.C) + Calculate(plate.A, plate.C, plate.D);
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

using System.Windows.Media.Media3D;

using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Extensions
{
    public static class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateRelativeReferenceGridAt(this ReferenceGrid baseGrid, double distance)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid()
            {
                //A = baseGrid.A.InwardNodeAlong(Vector3D.Multiply(distance, baseGrid.VectorA)),
                //B = baseGrid.B.InwardNodeAlong(Vector3D.Multiply(distance, baseGrid.VectorB)),
                //C = baseGrid.C.InwardNodeAlong(Vector3D.Multiply(distance, baseGrid.VectorC)),
                //D = baseGrid.D.InwardNodeAlong(Vector3D.Multiply(distance, baseGrid.VectorD))

                A = baseGrid.A.GetNewNodeBasedOn(baseGrid.VectorA, distance),
                B = baseGrid.B.GetNewNodeBasedOn(baseGrid.VectorB, distance),
                C = baseGrid.C.GetNewNodeBasedOn(baseGrid.VectorC, distance),
                D = baseGrid.D.GetNewNodeBasedOn(baseGrid.VectorD, distance)

            };

            return referenceGridAtSpecifiedDistance;
        }

        
    }

}

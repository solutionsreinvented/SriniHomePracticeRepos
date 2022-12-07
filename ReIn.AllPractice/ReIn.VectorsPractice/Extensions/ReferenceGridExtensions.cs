using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Extensions
{
    public static class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateRelativeReferenceGridAt(this ReferenceGrid baseReferenceGrid, double distance)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid()
            {
                A = baseReferenceGrid.A.CreateNodeBasedOn(baseReferenceGrid.VectorA, distance),
                B = baseReferenceGrid.B.CreateNodeBasedOn(baseReferenceGrid.VectorB, distance),
                C = baseReferenceGrid.C.CreateNodeBasedOn(baseReferenceGrid.VectorC, distance),
                D = baseReferenceGrid.D.CreateNodeBasedOn(baseReferenceGrid.VectorD, distance)

            };

            return referenceGridAtSpecifiedDistance;
        }

    }
}

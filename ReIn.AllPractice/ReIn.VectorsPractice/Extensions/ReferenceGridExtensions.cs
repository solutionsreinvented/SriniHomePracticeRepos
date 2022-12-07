using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Extensions
{
    public static class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateRelativeReferenceGridAt(this ReferenceGrid baseReferenceGrid, double distance)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid()
            {
                A = baseReferenceGrid.A.GetNewNodeBasedOn(baseReferenceGrid.VectorA, distance),
                B = baseReferenceGrid.B.GetNewNodeBasedOn(baseReferenceGrid.VectorB, distance),
                C = baseReferenceGrid.C.GetNewNodeBasedOn(baseReferenceGrid.VectorC, distance),
                D = baseReferenceGrid.D.GetNewNodeBasedOn(baseReferenceGrid.VectorD, distance)

            };

            return referenceGridAtSpecifiedDistance;
        }

    }
}

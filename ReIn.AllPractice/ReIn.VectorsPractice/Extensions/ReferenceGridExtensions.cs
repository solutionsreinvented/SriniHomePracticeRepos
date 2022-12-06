using System.Windows.Media.Media3D;

using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Extensions
{
    public static class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateRelativeReferenceGridAt(this ReferenceGrid baseGrid, double distance)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid(2)
            {
                A = InwardPoint3DFromNode(baseGrid.A, Vector3D.Multiply(distance, baseGrid.VectorA)),
                B = InwardPoint3DFromNode(baseGrid.B, Vector3D.Multiply(distance, baseGrid.VectorB)),
                C = InwardPoint3DFromNode(baseGrid.C, Vector3D.Multiply(distance, baseGrid.VectorC)),
                D = InwardPoint3DFromNode(baseGrid.D, Vector3D.Multiply(distance, baseGrid.VectorD))
            };

            return referenceGridAtSpecifiedDistance;
        }

        private static Node OutwardPoint3DToNode(Node node, Vector3D vector)
        {
            return new Node()
            {
                X = node.X + vector.X,
                Y = node.Y + vector.Y,
                Z = node.Z + vector.Z
            };
        }

        private static Node InwardPoint3DFromNode(Node node, Vector3D vector)
        {
            return new Node()
            {
                X = node.X - vector.X,
                Y = node.Y - vector.Y,
                Z = node.Z - vector.Z
            };
        }

    }

}

using System.Windows.Media.Media3D;

using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Extensions
{
    public static class NodeExtensions
    {
        public static Node GetNewNodeBasedOn(this Node referenceNode, Vector3D directionVector, double distanceFromReferenceNode)
        {
            Vector3D vectorAtDistance = Vector3D.Multiply(distanceFromReferenceNode, directionVector);

            return new Node()
            {
                X = referenceNode.X + vectorAtDistance.X,
                Y = referenceNode.Y + vectorAtDistance.Y,
                Z = referenceNode.Z + vectorAtDistance.Z
            };
        }

        public static Node OutwardNodeAlong(this Node referenceNode, Vector3D directionVector)
        {
            return new Node()
            {
                X = referenceNode.X + directionVector.X,
                Y = referenceNode.Y + directionVector.Y,
                Z = referenceNode.Z + directionVector.Z
            };
        }

        public static Node InwardNodeAlong(this Node referenceNode, Vector3D directionVector)
        {
            return new Node()
            {
                X = referenceNode.X - directionVector.X,
                Y = referenceNode.Y - directionVector.Y,
                Z = referenceNode.Z - directionVector.Z
            };
        }

    }
}

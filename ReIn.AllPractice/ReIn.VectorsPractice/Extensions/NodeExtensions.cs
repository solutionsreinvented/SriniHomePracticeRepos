using System.Windows.Media.Media3D;

using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Extensions
{
    public static class NodeExtensions
    {
        /// <summary>
        /// Creates a new <see cref="Node"/> at a proportionate distance from the reference node on the given <see cref="Vector3D"/>.
        /// </summary>
        /// <param name="referenceNode"><see cref="Node"/> from which the distance factor is measured.</param>
        /// <param name="directionVector">A full <see cref="Vector3D"/> vector in the direction in which a new <see cref="Node"/> 
        /// to be created. This shall not be a normalized vector.</param>
        /// <param name="distanceFactor">A factor indicating the proportionate distance with respect to the length/magnitude of the 
        /// given direction vector.</param>
        /// <returns>A new <see cref="Node"/> lying along the given vector.</returns>
        public static Node CreateNodeBasedOn(this Node referenceNode, Vector3D directionVector, double distanceFactor)
        {
            Vector3D proportionateVector = Vector3D.Multiply(distanceFactor, directionVector);

            return new Node()
            {
                X = referenceNode.X + proportionateVector.X,
                Y = referenceNode.Y + proportionateVector.Y,
                Z = referenceNode.Z + proportionateVector.Z
            };
        }
        /// <summary>
        /// Creates a <see cref="Vector3D"/> pointing from start node towards the end node.
        /// </summary>
        /// <param name="startNode">Start <see cref="Node"/> of the vector.</param>
        /// <param name="endNode">End <see cref="Node"/> of the vector.</param>
        /// <returns>A <see cref="Vector3D"/> from start node to end node.</returns>
        public static Vector3D VectorTowards(this Node startNode, Node endNode)
        {
            return new Vector3D(endNode.X - startNode.X, endNode.Y - startNode.Y, endNode.Z - startNode.Z);
        }
    }
}

using System;
using System.Windows;
using System.Windows.Media.Media3D;

using ReInvented.StaadPro.Interactivity.Entities;

namespace Flexture.GeometricCalculations
{

    public class AIFunctions
    {
        public static Node LinesIntersection(Member m1, Member m2)
        {
            Node intersectionNode = null;

            Vector3D v1 = m1.EndNode.ToVector - m1.StartNode.ToVector;
            Vector3D v2 = m2.EndNode.ToVector - m2.StartNode.ToVector;

            Vector3D cross = Vector3D.CrossProduct(v1, v2);

            // Check if the two lines are parallel
            if (cross.LengthSquared == 0)
            {
                _ = MessageBox.Show("The two lines are parallel and do not intersect.", "Intersect lines", MessageBoxButton.OK);
            }
            else
            {
                // Calculate the parameter values for the intersection point
                double t1 = Vector3D.CrossProduct(m2.StartNode.ToVector - m1.StartNode.ToVector, v2).Length / cross.Length;
                double t2 = Vector3D.CrossProduct(m2.StartNode.ToVector - m1.StartNode.ToVector, v1).Length / cross.Length;

                // Calculate the intersection point coordinates
                Vector3D p1 = m1.StartNode.ToVector + (t1 * v1);
                Vector3D p2 = m2.StartNode.ToVector + (t2 * v2);

                double lSquared = Math.Round(Vector3D.Subtract(p1, p2).LengthSquared, 3);

                if (lSquared == 0)
                {
                    intersectionNode = new Node
                    {
                        X = p1.X,
                        Y = p1.Y,
                        Z = p1.Z
                    };
                }

            }

            return intersectionNode;
        }

        public static Node RotateNode(Node targetNode, Node axisStartNode, Node axisEndNode, double rotateBy)
        {
            // Calculate the axis of rotation
            Vector3D axis = axisEndNode.ToVector - axisStartNode.ToVector;

            axis.Normalize();

            // Create a Quaternion for the rotation
            Quaternion rotation = new Quaternion(axis, rotateBy);

            // Rotate the specified node
            Vector3D rotatedNode = RotateVectorByQuaternion(targetNode.ToVector - axisStartNode.ToVector, rotation) + axisStartNode.ToVector;

            return new Node() { X = rotatedNode.X, Y = rotatedNode.Y, Z = rotatedNode.Z };
        }

        private static Vector3D RotateVectorByQuaternion(Vector3D v, Quaternion q)
        {
            // Create a Quaternion from the Vector3D
            Quaternion vq = new Quaternion(v.X, v.Y, v.Z, 0);

            // Rotate the Quaternion using the given Quaternion
            Quaternion firstRotate = q * vq;
            q.Conjugate();
            Quaternion rotated = firstRotate * q;

            // Convert the rotated Quaternion back to a Vector3D
            return new Vector3D(rotated.X, rotated.Y, rotated.Z);
        }

    }
}

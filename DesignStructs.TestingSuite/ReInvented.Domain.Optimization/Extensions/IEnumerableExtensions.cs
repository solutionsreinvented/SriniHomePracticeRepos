using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Shared;
using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.Domain.Optimization.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Node GetCenter(this IEnumerable<Node> nodesOnCircle, int roundRigits = 3)
        {
            Node center = new Node(Math.Round(nodesOnCircle.Average(n => n.X), roundRigits), Math.Round(nodesOnCircle.Average(n => n.Y), roundRigits), Math.Round(nodesOnCircle.Average(n => n.Z), roundRigits));
            return center;
        }

        public static List<(int NodeId, double Fx, double Fz)> DistributeTorque(this IEnumerable<Node> nodes, Node centre, double torqueToBeDistributed)
        {
            double radius = Math.Sqrt((nodes.First().X - centre.X).Squared() + (nodes.First().Z - centre.Z).Squared());
            double resultantForceAtEachNode = torqueToBeDistributed / (nodes.Count() * radius);

            List<(int NodeId, double Fx, double Fz)> torqueNodeDatas = new List<(int NodeId, double Fx, double Fz)>();

            foreach (Node n in nodes)
            {
                double angleToNode = (Math.Atan2(n.Z - centre.Z, n.X - centre.X).Degrees() + Constants.WholeCircleAngleInDegrees) % Constants.WholeCircleAngleInDegrees;
                double xComponent = resultantForceAtEachNode * Math.Sin(angleToNode.Radians());
                double zComponent = (-1.0) * resultantForceAtEachNode * Math.Cos(angleToNode.Radians());

                torqueNodeDatas.Add((n.Id, xComponent, zComponent));
            }

            return torqueNodeDatas;
        }
    }
}

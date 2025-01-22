using System;
using System.Collections.Generic;

using ReInvented.Shared;
using ReInvented.Shared.Extensions;
using ReInvented.StaadPro.Interop.Entities;

namespace PlateMeshing.Models
{
    public class CircularBasePlateConfiguration
    {
        public Node Center { get; set; }
        public double Radius { get; set; }
        public double Divisions { get; set; }
        public int BoltCount { get; set; }
        public double BoltHoleRadius { get; set; }
        public int BoltHoleDivisions { get; set; }
        public double BoltsPCD { get; set; }
        public double FirstBoltAngle { get; set; }
    }

    public class BoltHoleMesh
    {
        public BoltHoleMesh(int id, Node holeCenter, HashSet<Node> nodes)
        {
            Id = id;
            HoleCenter = holeCenter;
            Nodes = nodes;
        }

        public int Id { get; set; }

        public Node HoleCenter { get; set; }

        public HashSet<Node> Nodes { get; set; }

        public static BoltHoleMesh Generate(int id, Node reference, double rBoltHolePcd, double angle, double rBoltHole, int divsBoltHole, double dy, int lastNodeId)
        {
            double dx = rBoltHolePcd * Math.Cos(angle.Radians());
            double dz = rBoltHolePcd * Math.Sin(angle.Radians());

            Node bCenter = new Node(reference.X + dx, reference.Y + dy, reference.Z + dz);
            HashSet<Node> bNodes = Node.GenerateNodesOnCircularPath(bCenter, rBoltHole, divsBoltHole, 0.0, lastNodeId);

            return new BoltHoleMesh(id, bCenter, bNodes);
        }

        public static IEnumerable<BoltHoleMesh> GenerateAtPCD(Node reference, int nBolts, double sBoltAngle, double rBoltHolePcd, double rBoltHole, int divsBoltHole, double dy, int lastNodeId)
        {
            double boltIncAngle = Constants.WholeCircleAngleInDegrees / nBolts;
            List<BoltHoleMesh> boltHoleMeshes = new List<BoltHoleMesh>();

            for (int i = 0; i < nBolts; i++)
            {
                double angle = sBoltAngle + i * boltIncAngle;
                BoltHoleMesh mesh = Generate(i + 1, reference, rBoltHolePcd, angle, rBoltHole, divsBoltHole, dy, lastNodeId);

                boltHoleMeshes.Add(mesh);

                lastNodeId = mesh.Nodes.LastId();
            }

            return boltHoleMeshes;
        }
    }
}

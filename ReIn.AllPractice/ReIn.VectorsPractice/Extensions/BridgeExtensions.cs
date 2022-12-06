using System;

using ReIn.VectorsPractice.Models;

using ReInvented.Shared;
using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Extensions
{
    public static class BridgeExtensions
    {
        public static ReferenceGrid GetStartReferenceGrid(this Bridge bridge)
        {
            double rightAngle = bridge.Theta + (bridge.Alpha / 2);
            double leftAngle = bridge.Theta - (bridge.Alpha / 2);

            ReferenceGrid referenceGrid = new ReferenceGrid()
            {
                A = new Node()
                {
                    X = bridge.Origin.X + (bridge.Radius * Math.Cos(leftAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.Radius * Math.Sin(leftAngle.ToRadians()))
                },
                B = new Node()
                {
                    X = bridge.Origin.X + (bridge.Radius * Math.Cos(rightAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.Radius * Math.Sin(rightAngle.ToRadians()))
                }
            };

            referenceGrid.C = new Node(referenceGrid.A.X, referenceGrid.A.Y + bridge.Height, referenceGrid.A.Z);
            referenceGrid.D = new Node(referenceGrid.B.X, referenceGrid.B.Y + bridge.Height, referenceGrid.B.Z);

            return referenceGrid;
        }
    }
}

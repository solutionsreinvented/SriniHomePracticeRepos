using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Media3D;

using ReInvented.Shared;

namespace ReIn.VectorsPractice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProcessBridge();
        }

        public void ProcessBridge()
        {
            Bridge bridge = new Bridge()
            {
                Height = 3.0,
                TankRadius = 25.0,
                Theta = -60,
                Width = 2.5,
                DeckElevation = 10.0,
                Origin = new Point3D(0.0, 0.0, 0.0),
                ReferenceGrids = new List<ReferenceGrid>()
                {
                    new ReferenceGrid(){Spacing = 0},
                    new ReferenceGrid(){Spacing = 2.3},
                    new ReferenceGrid(){Spacing = 2.3}
                }
            };

            double cumulativeLength = 0.0;

            for (int i = 0; i < bridge.ReferenceGrids.Count; i++)
            {
                cumulativeLength += bridge.ReferenceGrids[i].Spacing;
                bridge.ReferenceGrids[i].Distance = cumulativeLength;
            }

            ReferenceGrid sRefGrid = GetStartReferenceGrid(bridge);

            Vector3D longBVector = sRefGrid.VectorB;


            List<ReferenceGrid> refGrids = new List<ReferenceGrid>();

            double spacing = bridge.Length / 10;

            for (int i = 0; i < bridge.Length / spacing; i++)
            {
                refGrids.Add(ReferenceGridExtensions.GenerateReferenceGrid(sRefGrid, i * spacing));
            }

            var pointHalf = sRefGrid.A + Vector3D.Multiply(0.5, sRefGrid.VectorAB);

            var gridAtGivenSpacing = ReferenceGridExtensions.GenerateReferenceGrid(sRefGrid, bridge.Length * 2);

        }

        private ReferenceGrid GetEndReferenceGrid()
        {
            throw new NotImplementedException();
        }

        private static ReferenceGrid GetStartReferenceGrid(Bridge bridge)
        {

            double planIncludedAngle = 2 * Math.Asin(bridge.Width / 2 / bridge.TankRadius).ToDegrees();

            double rightAngle = bridge.Theta + (planIncludedAngle / 2);
            double leftAngle = bridge.Theta - (planIncludedAngle / 2);

            ReferenceGrid referenceGrid = new ReferenceGrid()
            {
                A = new Point3D()
                {
                    X = bridge.Origin.X + (bridge.TankRadius * Math.Cos(leftAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.TankRadius * Math.Sin(leftAngle.ToRadians()))
                },
                B = new Point3D()
                {
                    X = bridge.Origin.X + (bridge.TankRadius * Math.Cos(rightAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.TankRadius * Math.Sin(rightAngle.ToRadians()))
                }
            };

            referenceGrid.C = new Point3D(referenceGrid.A.X, referenceGrid.A.Y + bridge.Height, referenceGrid.A.Z);
            referenceGrid.D = new Point3D(referenceGrid.B.X, referenceGrid.B.Y + bridge.Height, referenceGrid.B.Z);

            return referenceGrid;
        }

    }

    class ReferenceGrid
    {
        public ReferenceGrid()
        {

        }

        #region Public Properties

        public double Spacing { get; set; }

        public double Distance { get; set; }
        /// <summary>
        /// Point to the left of the feed pipe and at the bottom chord level
        /// </summary>
        public Point3D A { get; set; }
        /// <summary>
        /// Point to the right of the feed pipe and at the bottom chord level
        /// </summary>
        public Point3D B { get; set; }
        /// <summary>
        /// Point to the left of the feed pipe and at the top chord level
        /// </summary>
        public Point3D C { get; set; }
        /// <summary>
        /// Point to the right of the feed pipe and at the top chord level
        /// </summary>
        public Point3D D { get; set; }

        #endregion

        #region Read-only Properties

        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from A to B.
        /// </summary>
        public Vector3D VectorAB => new Vector3D(B.X - A.X, B.Y - A.Y, B.Z - A.Z);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from C to D.
        /// </summary>
        public Vector3D VectorCD => new Vector3D(D.X - C.X, D.Y - C.Y, D.Z - C.Z);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from A to C.
        /// </summary>
        public Vector3D VectorAC => new Vector3D(C.X - A.X, C.Y - A.Y, C.Z - A.Z);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from B to D.
        /// </summary>
        public Vector3D VectorBD => new Vector3D(D.X - B.X, D.Y - B.Y, D.Z - B.Z);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from B to A.
        /// </summary>
        public Vector3D VectorBA => Vector3D.Multiply(-1, VectorAB);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from D to C.
        /// </summary>
        public Vector3D VectorDC => Vector3D.Multiply(-1, VectorCD);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from C to A.
        /// </summary>
        public Vector3D VectorCA => Vector3D.Multiply(-1, VectorAC);
        /// <summary>
        /// Provides the <see cref="Vector3D"/> pointing from D to B.
        /// </summary>
        public Vector3D VectorDB => Vector3D.Multiply(-1, VectorBD);
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> pointing towards the origin at point A.
        /// </summary>
        public Vector3D VectorA => NormalizedCrossProductVectorFrom(VectorAB, VectorAC);
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> pointing towards the origin at point B.
        /// </summary>
        public Vector3D VectorB => NormalizedCrossProductVectorFrom(VectorBD, VectorBA);
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> pointing towards the origin at point C.
        /// </summary>
        public Vector3D VectorC => NormalizedCrossProductVectorFrom(VectorCA, VectorCD);
        /// <summary>
        /// Provides a normalized <see cref="Vector3D"/> pointing towards the origin at point D.
        /// </summary>
        public Vector3D VectorD => NormalizedCrossProductVectorFrom(VectorDC, VectorDB);

        #endregion

        #region Private Helpers

        private Vector3D NormalizedCrossProductVectorFrom(Vector3D vectorA, Vector3D vectorB)
        {
            Vector3D crossProductVector = Vector3D.CrossProduct(vectorA, vectorB);
            crossProductVector.Normalize();

            return crossProductVector;
        }

        #endregion

    }

    class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateReferenceGrid(ReferenceGrid baseReferenceGrid, double distanceFromBaseGrid)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid()
            {
                A = baseReferenceGrid.A - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorA),
                B = baseReferenceGrid.B - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorB),
                C = baseReferenceGrid.C - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorC),
                D = baseReferenceGrid.D - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorD)
            };

            return referenceGridAtSpecifiedDistance;
        }
    }


    class Bridge
    {
        public Bridge()
        {
            ReferenceGrids = new List<ReferenceGrid>();
        }

        public List<ReferenceGrid> ReferenceGrids { get; set; }

        public Point3D Origin { get; set; }

        public double Length => TankRadius * Math.Cos((Alpha / 2).ToRadians());

        public double Width { get; set; }

        public double Height { get; set; }

        public double Theta { get; set; }

        public double Alpha => 2 * Math.Asin(Width / 2 / TankRadius).ToDegrees();


        public double TankRadius { get; set; }

        public double DeckElevation { get; set; }


    }


}

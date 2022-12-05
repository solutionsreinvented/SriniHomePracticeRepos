using System;
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
                Theta = 80,
                Width = 1.5,
                DeckElevation = 10.0,
                Origin = new Point3D(0.0, 0.0, 0.0)
            };

            double bridgePlanAngle = 2 * Math.Asin(bridge.Width / 2 / bridge.TankRadius).ToDegrees();

            double rightAngle;
            double leftAngle;

            if (bridge.Theta >= 180)
            {
                rightAngle = bridge.Theta - (bridgePlanAngle / 2);
                leftAngle = bridge.Theta + (bridgePlanAngle / 2);
            }
            else
            {
                rightAngle = bridge.Theta + (bridgePlanAngle / 2);
                leftAngle = bridge.Theta - (bridgePlanAngle / 2);
            }


            bridge.ReferenceGrid = new ReferenceGrid()
            {
                A = new Point3D()
                {
                    X = bridge.Origin.X + bridge.TankRadius * Math.Cos(leftAngle.ToRadians()),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + bridge.TankRadius * Math.Sin(leftAngle.ToRadians())
                },
                B = new Point3D()
                {
                    X = bridge.Origin.X + bridge.TankRadius * Math.Cos(rightAngle.ToRadians()),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + bridge.TankRadius * Math.Sin(rightAngle.ToRadians())
                }
            };

            bridge.ReferenceGrid.C = new Point3D(bridge.ReferenceGrid.A.X, bridge.ReferenceGrid.A.Y + bridge.Height, bridge.ReferenceGrid.A.Z);
            bridge.ReferenceGrid.D = new Point3D(bridge.ReferenceGrid.B.X, bridge.ReferenceGrid.B.Y + bridge.Height, bridge.ReferenceGrid.B.Z);



            ///MessageBox.Show($"A ({bridge.ReferenceGrid.A.X}, {bridge.ReferenceGrid.A.Y}, {bridge.ReferenceGrid.A.Z})");
            ///MessageBox.Show($"B ({bridge.ReferenceGrid.B.X}, {bridge.ReferenceGrid.B.Y}, {bridge.ReferenceGrid.B.Z})");


        }

    }

    class ReferenceGrid
    {
        public ReferenceGrid()
        {

        }
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

    }

    class Bridge
    {
        public Bridge()
        {

        }

        public ReferenceGrid ReferenceGrid { get; set; }

        public Point3D Origin { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Theta { get; set; }

        public double TankRadius { get; set; }

        public double DeckElevation { get; set; }

    }


}

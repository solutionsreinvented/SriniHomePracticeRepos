using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Media3D;

using ReInvented.Shared;
using ReInvented.Thickener.Domain.Entities;

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
                Height = 2.8,
                TankRadius = 25.0,
                Theta = 225,
                Width = 2.0,
                DeckElevation = 10.0,
                Origin = new Point3D(0.0, 0.0, 0.0),
                ReferenceGrids = new List<ReferenceGrid>()
                {
                    new ReferenceGrid(1){Spacing = 0},
                    new ReferenceGrid(2){Spacing = 2.3},
                    new ReferenceGrid(3){Spacing = 2.3}
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


            List<IReferenceGrid> refGrids = new List<IReferenceGrid>();

            double spacing = bridge.Length / 10;
            int grids = (int)(bridge.Length / spacing);

            for (int i = 0; i <= grids; i++)
            {
                refGrids.Add(ReferenceGridExtensions.GenerateReferenceGrid(sRefGrid, i * spacing));
            }

            ILongitudinalFrameVectors longitudinalFrameVectors = refGrids[0] as ILongitudinalFrameVectors;

            var vectorA = longitudinalFrameVectors.VectorA;

            ///var totalNodes = refGrids.Count * 4;

            string syntax = $"JOINT COORDINATES{Environment.NewLine}";

            int nodeId = 1;

            for (int i = 0; i < refGrids.Count; i++)
            {
                refGrids[i].A.Id = nodeId++;
                refGrids[i].B.Id = nodeId++;
                refGrids[i].C.Id = nodeId++;
                refGrids[i].D.Id = nodeId++;

                syntax += $"{refGrids[i].A.Id} {refGrids[i].A.X} {refGrids[i].A.Y} {refGrids[i].A.Z};{Environment.NewLine}";
                syntax += $"{refGrids[i].B.Id} {refGrids[i].B.X} {refGrids[i].B.Y} {refGrids[i].B.Z};{Environment.NewLine}";
                syntax += $"{refGrids[i].C.Id} {refGrids[i].C.X} {refGrids[i].C.Y} {refGrids[i].C.Z};{Environment.NewLine}";
                syntax += $"{refGrids[i].D.Id} {refGrids[i].D.X} {refGrids[i].D.Y} {refGrids[i].D.Z};{Environment.NewLine}";
            }

            syntax += $"MEMBER INCIDENCES{Environment.NewLine}";

            int memberId = 1;


            for (int i = 0; i < refGrids.Count; i++)
            {
                syntax += $"{memberId++} {refGrids[i].A.Id} {refGrids[i].B.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {refGrids[i].C.Id} {refGrids[i].D.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {refGrids[i].A.Id} {refGrids[i].C.Id};{Environment.NewLine}";
                syntax += $"{memberId++} {refGrids[i].B.Id} {refGrids[i].D.Id};{Environment.NewLine}";
                if (i >= 1)
                {
                    syntax += $"{memberId++} {refGrids[i - 1].A.Id} {refGrids[i].A.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {refGrids[i - 1].B.Id} {refGrids[i].B.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {refGrids[i - 1].C.Id} {refGrids[i].C.Id};{Environment.NewLine}";
                    syntax += $"{memberId++} {refGrids[i - 1].D.Id} {refGrids[i].D.Id};{Environment.NewLine}";
                }
            }


            File.WriteAllText(@"C:\Users\masanams\Desktop\Test Files\bridge-geometry.txt", syntax);

            ///var pointHalf = sRefGrid.A + Vector3D.Multiply(0.5, sRefGrid.VectorAB);

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

            ReferenceGrid referenceGrid = new ReferenceGrid(1)
            {
                A = new Node()
                {
                    X = bridge.Origin.X + (bridge.TankRadius * Math.Cos(leftAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.TankRadius * Math.Sin(leftAngle.ToRadians()))
                },
                B = new Node()
                {
                    X = bridge.Origin.X + (bridge.TankRadius * Math.Cos(rightAngle.ToRadians())),
                    Y = bridge.Origin.Y + bridge.DeckElevation,
                    Z = bridge.Origin.Z + (-1) * (bridge.TankRadius * Math.Sin(rightAngle.ToRadians()))
                }
            };

            referenceGrid.C = new Node(referenceGrid.A.X, referenceGrid.A.Y + bridge.Height, referenceGrid.A.Z);
            referenceGrid.D = new Node(referenceGrid.B.X, referenceGrid.B.Y + bridge.Height, referenceGrid.B.Z);

            return referenceGrid;
        }

    }

    class ReferenceGridExtensions
    {
        public static ReferenceGrid GenerateReferenceGrid(ReferenceGrid baseReferenceGrid, double distanceFromBaseGrid)
        {
            ReferenceGrid referenceGridAtSpecifiedDistance = new ReferenceGrid(2)
            {
                ///A = baseReferenceGrid.A - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorA),
                ///B = baseReferenceGrid.B - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorB),
                ///C = baseReferenceGrid.C - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorC),
                ///D = baseReferenceGrid.D - Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorD)
                A = SubtractPoint3DFromNode(baseReferenceGrid.A, Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorA)),
                B = SubtractPoint3DFromNode(baseReferenceGrid.B, Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorB)),
                C = SubtractPoint3DFromNode(baseReferenceGrid.C, Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorC)),
                D = SubtractPoint3DFromNode(baseReferenceGrid.D, Vector3D.Multiply(distanceFromBaseGrid, baseReferenceGrid.VectorD))
            };

            return referenceGridAtSpecifiedDistance;
        }

        private static Node AddPoint3DToNode(Node node, Vector3D point3D)
        {
            return new Node()
            {
                X = node.X + point3D.X,
                Y = node.Y + point3D.Y,
                Z = node.Z + point3D.Z
            };
        }

        private static Node SubtractPoint3DFromNode(Node node, Vector3D point3D)
        {
            return new Node()
            {
                X = node.X - point3D.X,
                Y = node.Y - point3D.Y,
                Z = node.Z - point3D.Z
            };
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

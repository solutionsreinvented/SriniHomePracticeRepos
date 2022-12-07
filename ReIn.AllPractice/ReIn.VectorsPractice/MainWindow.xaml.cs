using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

using ReIn.VectorsPractice.Base;
using ReIn.VectorsPractice.Extensions;
using ReIn.VectorsPractice.Fakes;
using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;
using ReIn.VectorsPractice.Services;

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
            Bridge bridge = FakeBridgeFactory.Generate();

            double cumulativeLength = 0.0;

            for (int i = 0; i < bridge.FrameGrids.Count; i++)
            {
                cumulativeLength += bridge.FrameGrids[i].Spacing;
                bridge.FrameGrids[i].Distance = cumulativeLength;
            }

            ReferenceGrid sRefGrid = bridge.GetStartReferenceGrid();

            for (int i = 0; i < bridge.FrameGrids.Count; i++)
            {
                bridge.FrameGrids[i].ReferenceGrid = sRefGrid.GenerateRelativeReferenceGridAt(-bridge.FrameGrids[i].Distance);
                GenerateFrameGridNodes(bridge, bridge.FrameGrids[i] as FrameGrid);
            }

            List<IReferenceGrid> refGrids = bridge.FrameGrids.Cast<IReferenceGrid>().ToList();

            string syntax = SyntaxGenerationService.NodeIncidencesFrom(refGrids) +
                            SyntaxGenerationService.MemberIncidencesFrom(refGrids);


            File.WriteAllText(@"C:\Users\masanams\Desktop\Test Files\bridge-geometry.txt", syntax);

        }

        private void GenerateFrameGridNodes(Bridge bridge, FrameGrid frameGrid)
        {
            ICrossFrameVectors referenceGrid = frameGrid.ReferenceGrid as ICrossFrameVectors;
            ReferenceGrid baseGrid = new ReferenceGrid();
            double shift = (frameGrid.Width - bridge.Width) / 2;

            frameGrid.A = Vector3DToNode(frameGrid.ReferenceGrid.A, Vector3D.Multiply((-1) * shift / bridge.Width, referenceGrid.VectorAB));
            frameGrid.B = Vector3DToNode(frameGrid.ReferenceGrid.A, Vector3D.Multiply((bridge.Width + shift) / bridge.Width, referenceGrid.VectorAB));
            frameGrid.C = Vector3DToNode(frameGrid.ReferenceGrid.C, Vector3D.Multiply((-1) * shift / bridge.Width, referenceGrid.VectorCD));
            frameGrid.D = Vector3DToNode(frameGrid.ReferenceGrid.C, Vector3D.Multiply((bridge.Width + shift) / bridge.Width, referenceGrid.VectorCD));
        }

        private Node Vector3DToNode(Node referenceNode, Vector3D vector)
        {
            return new Node() { X = referenceNode.X + vector.X, Y = referenceNode.Y + vector.Y, Z = referenceNode.Z + vector.Z };
        }

    }

}

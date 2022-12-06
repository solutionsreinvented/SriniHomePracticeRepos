using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Media3D;

using ReIn.VectorsPractice.Extensions;
using ReIn.VectorsPractice.Fakes;
using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;
using ReIn.VectorsPractice.Services;

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

            for (int i = 0; i < bridge.ReferenceGrids.Count; i++)
            {
                cumulativeLength += bridge.ReferenceGrids[i].Spacing;
                bridge.ReferenceGrids[i].Distance = cumulativeLength;
            }

            ReferenceGrid sRefGrid = bridge.GetStartReferenceGrid();

            List<IReferenceGrid> refGrids = new List<IReferenceGrid>();

            double spacing = bridge.Length / 10;
            int grids = (int)(bridge.Length / spacing);

            for (int i = 0; i <= grids; i++)
            {
                refGrids.Add(sRefGrid.GenerateRelativeReferenceGridAt(i * spacing));
            }


            string syntax = SyntaxGenerationService.NodeIncidencesFrom(refGrids) +
                            SyntaxGenerationService.MemberIncidencesFrom(refGrids);


            File.WriteAllText(@"C:\Users\masanams\Desktop\Test Files\bridge-geometry.txt", syntax);

        }

    }

}

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
        private void GetCount()
        {
            DateTime startDate = new DateTime(2019, 01, 11);
            DateTime endDate = new DateTime(2022, 02, 11);

            int nYears = endDate.Year - startDate.Year;
            int dayOfMonth = 11;

            List<DateTime> dates = new List<DateTime>();

            if (nYears == 0)
            {
                dates = PartialYearCounter.GetCount(startDate, endDate, dayOfMonth);
            }
            else
            {
                dates.AddRange(PartialYearCounter.GetCount(startDate, new DateTime(startDate.Year, 12, 31), dayOfMonth));

                for (int yearCount = 1; yearCount < nYears; yearCount++)
                {
                    dates.AddRange(CompleteYearCounter.GetCount(startDate.Year + yearCount, dayOfMonth));
                }

                dates.AddRange(PartialYearCounter.GetCount(new DateTime(endDate.Year, 01, 01), endDate, dayOfMonth));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            GetCount();





            ///ProcessBridge();
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
                bridge.FrameGrids[i].GenerateFrameGridNodes(bridge.Width);
                ///GenerateFrameGridNodes(bridge.FrameGrids[i] as FrameGrid, bridge);
            }

            List<IReferenceGrid> refGrids = bridge.FrameGrids.Cast<IReferenceGrid>().ToList();

            string syntax = SyntaxGenerationService.NodeIncidencesFrom(refGrids) +
                            SyntaxGenerationService.MemberIncidencesFrom(refGrids);

            File.WriteAllText(@"C:\Users\masanams\Desktop\Test Files\bridge-geometry.txt", syntax);

        }

        private void GenerateFrameGridNodes(FrameGrid frameGrid, Bridge bridge)
        {
            IReferenceGrid referenceGrid = frameGrid.ReferenceGrid;
            ICrossFrameVectors crossFrameVectors = referenceGrid as ICrossFrameVectors;

            double shift = (frameGrid.Width - bridge.Width) / 2;
            double forwardLengthFactor = (bridge.Width + shift) / bridge.Width;
            double backwardLengthFactor = (-1) * shift / bridge.Width;

            frameGrid.A = referenceGrid.A.CreateNodeBasedOn(crossFrameVectors.VectorAB, backwardLengthFactor);
            frameGrid.B = referenceGrid.A.CreateNodeBasedOn(crossFrameVectors.VectorAB, forwardLengthFactor);
            frameGrid.C = referenceGrid.C.CreateNodeBasedOn(crossFrameVectors.VectorCD, backwardLengthFactor);
            frameGrid.D = referenceGrid.C.CreateNodeBasedOn(crossFrameVectors.VectorCD, forwardLengthFactor);
        }



    }

}

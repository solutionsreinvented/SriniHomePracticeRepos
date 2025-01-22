using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using OpenSTAADUI;

using PlateMeshing.Enums;
using PlateMeshing.Extensions;

using ReInvented.Shared;
using ReInvented.Shared.Extensions;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Services;

namespace PlateMeshing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string staadPath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Delete\ParamSurfaces\Param.std";
            double rBP = 1.05;
            int divsBP = 64;
            int nBolts = 10;
            double rBoltHole = 0.015;
            int divsBoltHole = 24;
            double pcdBolts = 2 * 0.975;

            string surfaceName = "Base Plate";

            OpenStaadWrapper wrapper = OpenStaadWrapperProvider.Get(staadPath);
            OpenSTAAD openStaad = wrapper.OpenStaad;
            OSGeometryUI geometry = wrapper.Geometry;

            openStaad.SetSilentMode(true);


            int lastNodeId = 1;

            Node center = new Node(lastNodeId, 0.0, 0.0, 0.0);

            int surfaceId = geometry.GenerateCircularBasePlateWithEccentricHole(surfaceName, center, rBP, divsBP, nBolts, rBoltHole,
                pcdBolts, divsBoltHole, 15.0, 0.3, 0, 0.0, 0.4, 32, lastNodeId);
            geometry.AddCircularRegionToSurfaceExt(surfaceId, center, 0.9, 25, 1, RegionType.Opening);
            geometry.CommitParametricSurfaceMesh(surfaceId);

            //geometry.CreateSolidCircularPlate(surfaceName, densityPoint, 1.8, nPoints, lastNodeId, AutoGenerate.No);

            //var surfacesCount = geometry.GetParametricSurfaceCount();


            //OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, nodesOpening, 1);
            //geometry.CreateNode(densityPoint.Id, densityPoint.X, densityPoint.Y, densityPoint.Z);

            //openStaad.SaveModel(true);


            //geometry.AddDensityPointToSurfaceExt(surfaceId, densityPoint, 1);
            //int regionResult = geometry.AddCircularRegionToSurfaceExt(surfaceId, densityPoint, rOpening, nPointsOpening, 1, RegionType.Region);

        }

    }
}

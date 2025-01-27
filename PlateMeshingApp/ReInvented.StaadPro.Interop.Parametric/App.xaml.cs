using System.Collections.Generic;
using System.Windows;

using OpenSTAADUI;

using ReInvented.Shared.Extensions;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.Interop.Parametric.Enums;
using ReInvented.StaadPro.Interop.Services;

namespace ReInvented.StaadPro.Interop.Parametric
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
            OpenStaadWrapper wrapper = OpenStaadWrapperProvider.Get(staadPath);
            OpenSTAAD openStaad = wrapper.OpenStaad;
            OSGeometryUI geometry = wrapper.Geometry;


            Node center = new Node(1, 0, 0, 0);
            HashSet<Node> periphery = Node.GenerateNodesOnCircularPath(center, 3.5, 80, lastUsedId: 1);
            HashSet<Node> opening = Node.GenerateNodesOnCircularPath(center, 2.5, 60, lastUsedId: periphery.LastId());
            HashSet<Node> inner = Node.GenerateNodesOnCircularPath(center, 1.5, 40, lastUsedId: opening.LastId());
            HashSet<Node> innermost = Node.GenerateNodesOnCircularPath(center, 0.5, 20, lastUsedId: inner.LastId());


            int surfaceId = geometry.DefineParametricSurfaceExt("Circular Base Plate", ParametericSurfaceType.None, periphery);
            geometry.AddPolygonalRegionToSurfaceExt(surfaceId, opening, RegionType.Opening);
            geometry.AddPolygonalRegionToSurfaceExt(surfaceId, inner, RegionType.Region);
            geometry.AddPolygonalRegionToSurfaceExt(surfaceId, innermost, RegionType.Opening);


            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, periphery, 1);
            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, opening, 1);
            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, inner, 1);
            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, innermost, 1);

            geometry.AddParametricSurfaceToModel(surfaceId);
            geometry.CommitParametricSurfaceMesh(surfaceId);

            //CircularBasePlateWithBoltHoles();

            //geometry.CreateSolidCircularPlate(surfaceName, densityPoint, 1.8, nPoints, lastNodeId, AutoGenerate.No);

            //var surfacesCount = geometry.GetParametricSurfaceCount();


            //OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, nodesOpening, 1);
            //geometry.CreateNode(densityPoint.Id, densityPoint.X, densityPoint.Y, densityPoint.Z);

            //openStaad.SaveModel(true);


            //geometry.AddDensityPointToSurfaceExt(surfaceId, densityPoint, 1);
            //int regionResult = geometry.AddCircularRegionToSurfaceExt(surfaceId, densityPoint, rOpening, nPointsOpening, 1, RegionType.Region);

        }

        private static void CircularBasePlateWithBoltHoles(string staadPath, OpenSTAAD openStaad, OSGeometryUI geometry)
        {
            double rBP = 1.05;
            int divsBP = 64;
            int nBolts = 10;
            double rBoltHole = 0.015;
            int divsBoltHole = 24;
            double pcdBolts = 2 * 0.975;

            string surfaceName = "Base Plate";

            openStaad.SetSilentMode(true);


            int lastNodeId = 1;

            Node center = new Node(lastNodeId, 0.0, 0.0, 0.0);

            int surfaceId = geometry.GenerateCircularBasePlateWithEccentricHole(surfaceName, center, rBP, divsBP, nBolts, rBoltHole,
                pcdBolts, divsBoltHole, 15.0, 0.3, -110.0, 0.3, 0.4, 32, lastNodeId);
        }
    }
}

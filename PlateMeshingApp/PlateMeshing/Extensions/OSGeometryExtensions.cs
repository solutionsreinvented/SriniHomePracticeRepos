using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using PlateMeshing.Enums;
using PlateMeshing.Models;

using ReInvented.Shared;
using ReInvented.Shared.Extensions;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;

namespace PlateMeshing.Extensions
{
    public static class OSGeometryExtensions
    {
        public static int CreateSolidCircularPlate(this OSGeometryUI geometry, string surfaceName, Node center,
            double radius, int divisions, int lastUsedNodeId = 0, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            HashSet<Node> nodes = Node.GenerateNodesOnCircularPath(center, radius, divisions, 0.0, lastUsedNodeId);
            return geometry.CreateSolidCircularPlate(surfaceName, center, nodes, autoGenerate);
        }

        public static int CreateSolidCircularPlate(this OSGeometryUI geometry, string surfaceName, Node center, IEnumerable<Node> vertices, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            List<Node> verticesList = vertices.ToList();

            Node sVertex = verticesList[0];
            Node xVertex = verticesList[1];
            Node yVertex = verticesList[2];

            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, vertices.ToHashSet(), 1);

            int surfaceId = geometry.DefineParametricSurfaceExt(surfaceName, ParametericSurfaceType.None, sVertex, xVertex, yVertex, vertices, autoGenerate);

            geometry.AddDensityPointToSurfaceExt(surfaceId, center, 1);
            geometry.AddParametricSurfaceToModel(surfaceId);
            //geometry.CommitParametricSurfaceMesh(surfaceId);

            return surfaceId;
        }

        public static int DefineParametricSurfaceExt(this OSGeometryUI geometry, string surfaceName, ParametericSurfaceType type,
            Node origin, Node xVertex, Node yVertex, IEnumerable<Node> vertices, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            int nVertices = vertices.Count();
            int[] verticesIds = vertices.Select(v => v.Id).ToArray();

            return geometry.DefineParametricSurface(surfaceName, type, origin.Id, xVertex.Id, yVertex.Id, nVertices, verticesIds, autoGenerate);
        }

        public static OSGeometryUI AddDensityLineToSurfaceExt(this OSGeometryUI geometry,
            int surfaceId, Node sNodeDensityLine, Node eNodeDensityLine, int sDensity, int eDensity, int nDivisions)
        {
            geometry.AddDensityLineToSurface(surfaceId, sNodeDensityLine.X, sNodeDensityLine.Y, sNodeDensityLine.Z, sDensity, eNodeDensityLine.X, eNodeDensityLine.Y, eNodeDensityLine.Z, eDensity, nDivisions);

            return geometry;
        }

        public static OSGeometryUI AddDensityPointToSurfaceExt(this OSGeometryUI geometry,
            int surfaceId, Node densityPoint, int density)
        {
            geometry.AddDensityPointToSurface(surfaceId, densityPoint.X, densityPoint.Y, densityPoint.Z, density);
            return geometry;
        }

        public static int AddPolygonalRegionToSurfaceExt(this OSGeometryUI geometry, int surfaceId, IEnumerable<Node> polygonNodes, RegionType regionType)
        {
            HashSet<Node> vertices = polygonNodes.ToHashSet();
            int nVertices = vertices.Count();
            object xArrayRegion = vertices.Select(n => n.X).ToArray();
            object yArrayRegion = vertices.Select(n => n.Y).ToArray();
            object zArrayRegion = vertices.Select(n => n.Z).ToArray();

            object densities = Enumerable.Repeat(1, nVertices).ToArray();
            object edgeDivs = Enumerable.Repeat(1, nVertices).ToArray();

            int result = geometry.AddPolygonalRegionToSurface(surfaceId, nVertices, xArrayRegion, yArrayRegion, zArrayRegion, densities, edgeDivs, regionType);

            return result;
        }

        public static int AddCircularRegionToSurfaceExt(this OSGeometryUI geometry, int surfaceId, Node center, double radius, int divisions, RegionType regionType, int lastUsedId = 0)
        {
            HashSet<Node> nodes = Node.GenerateNodesOnCircularPath(center, radius, divisions, 0.0, lastUsedId);
            int result = geometry.AddPolygonalRegionToSurfaceExt(surfaceId, nodes, regionType);

            return result;
        }

        public static int AddCircularRegionToSurfaceExt(this OSGeometryUI geometry, int surfaceId, Node center, double radius, int divisions, int density, RegionType regionType)
        {
            int result = geometry.AddCircularRegionToSurface(surfaceId, center.X, center.Y, center.Z, radius, divisions, density, regionType);

            return result;
        }

        public static int GenerateCircularBasePlate(this OSGeometryUI geometry, string surfaceName, Node center,
            double rBP, int divsBP, int nBolts, double rBoltHole, double pcdBolts, int divsBoltHole, double sAngleBolt, int lastUsedId = 0)
        {
            int lastNodeId = lastUsedId;
            double rBoltHolePcd = pcdBolts / 2;
            double boltIncAngle = Constants.WholeCircleAngleInDegrees / nBolts;

            HashSet<Node> nodes = Node.GenerateNodesOnCircularPath(center, rBP, divsBP, 0, lastNodeId);
            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, nodes, 1);

            List<Node> nodesList = nodes.ToList();

            int bpSurfaceId = geometry.DefineParametricSurfaceExt(surfaceName, ParametericSurfaceType.None, nodesList[0], nodesList[1], nodesList[2], nodes);
            geometry.AddDensityPointToSurfaceExt(bpSurfaceId, center, 1);


            lastNodeId = nodes.LastId();

            var holeMeshCollection = BoltHoleMesh.GenerateAtPCD(center, nBolts, sAngleBolt, rBoltHolePcd, rBoltHole, divsBoltHole, 0.0, lastNodeId);

            holeMeshCollection.ToList().ForEach(hm => geometry.AddPolygonalRegionToSurfaceExt(bpSurfaceId, hm.Nodes, RegionType.Opening));

            geometry.AddParametricSurfaceToModel(bpSurfaceId);
            geometry.CommitParametricSurfaceMesh(bpSurfaceId);

            return bpSurfaceId;
        }

        public static int GenerateCircularBasePlateWithEccentricHole(this OSGeometryUI geometry, string surfaceName, Node center,
            double rBP, int divsBP, int nBolts, double rBoltHole, double pcdBolts, int divsBoltHole, double sAngleBolt,
            double rEccentricHoleCenter, double angleEccentricHole, double dyEccentricHole, double rEccentricHole, int divsEccentricHole, int lastUsedId = 0)
        {
            int lastNodeId = lastUsedId;
            double rBoltHolePcd = pcdBolts / 2;
            double boltIncAngle = Constants.WholeCircleAngleInDegrees / nBolts;

            HashSet<Node> nodes = Node.GenerateNodesOnCircularPath(center, rBP, divsBP, 0, lastNodeId);
            OSGeometryExtensionsParallel.CreateMultipleNodes(geometry, nodes, 1);

            List<Node> nodesList = nodes.ToList();

            int bpSurfaceId = geometry.DefineParametricSurfaceExt(surfaceName, ParametericSurfaceType.None, nodesList[0], nodesList[1], nodesList[2], nodes);

            lastNodeId = nodes.LastId();

            var holeMeshCollection = BoltHoleMesh.GenerateAtPCD(center, nBolts, sAngleBolt, rBoltHolePcd, rBoltHole, divsBoltHole, 0.0, lastNodeId);

            holeMeshCollection.ToList().ForEach(hm => geometry.AddPolygonalRegionToSurfaceExt(bpSurfaceId, hm.Nodes, RegionType.Opening));

            lastNodeId = holeMeshCollection.SelectMany(hm => hm.Nodes).LastId();

            double xEccentricHoleCenter = center.X + rEccentricHoleCenter * Math.Cos(angleEccentricHole.Radians());
            double yEccentricHoleCenter = center.Y + dyEccentricHole;
            double zEccentricHoleCenter = center.Z + rEccentricHoleCenter * Math.Sin(angleEccentricHole.Radians());

            Node eccentricHoleCenter = new Node(xEccentricHoleCenter, yEccentricHoleCenter, zEccentricHoleCenter);

            HashSet<Node> eccentricHoleNodes = Node.GenerateNodesOnCircularPath(eccentricHoleCenter, rEccentricHole, divsEccentricHole, 0.0, lastUsedId);

            geometry.AddPolygonalRegionToSurfaceExt(bpSurfaceId, eccentricHoleNodes, RegionType.Opening);

            geometry.AddParametricSurfaceToModel(bpSurfaceId);
            geometry.CommitParametricSurfaceMesh(bpSurfaceId);

            return bpSurfaceId;
        }
    }
}

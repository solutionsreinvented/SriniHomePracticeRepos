using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Shared;
using ReInvented.Shared.Extensions;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Parametric.Enums;
using ReInvented.StaadPro.Interop.Parametric.Models;

namespace ReInvented.StaadPro.Interop.Extensions
{
    public static class OSGeometryExtensions
    {

        public static void AddAndCommitParametricSurfaceToModel(this OSGeometryUI geometry,  int surfaceId)
        {
            geometry.AddParametricSurfaceToModel(surfaceId);
            geometry.CommitParametricSurfaceMesh(surfaceId);
        }



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

            int surfaceId = geometry.DefineParametricSurfaceExt(surfaceName, sVertex, xVertex, yVertex, vertices, SurfaceType.None, autoGenerate);

            geometry.AddDensityPointToSurfaceExt(surfaceId, center, 1);
            geometry.AddParametricSurfaceToModel(surfaceId);
            //geometry.CommitParametricSurfaceMesh(surfaceId);

            return surfaceId;
        }

        /// <summary>
        /// Defines an annular parametric surface using the outer and inner polygon points (vertices) provided.
        /// </summary>
        /// <param name="geometry"><see cref="OSGeometryUI"/> COM object to carry out geometry operations in Staad.</param>
        /// <param name="surfaceName">Name of the parametric surface to be created.</param>
        /// <param name="outerVertices">Polygon points on the outer boundary. These nodes must exist in the model already.</param>
        /// <param name="innerVertices">Polygon points on the inner boundary. These nodes must exist in the model already.</param>
        /// <param name="type">Type of the surface being created. Refer <see cref="SurfaceType"/>.</param>
        /// <param name="autoGenerate"></param>
        /// <returns>An integer id of the created surface. A value of -1 indicates the surface is not created.</returns>
        public static int DefineAnnularParametricSurfaceExt(this OSGeometryUI geometry, string surfaceName,
            IEnumerable<Node> outerVertices, IEnumerable<Node> innerVertices,
            SurfaceType type = SurfaceType.None, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            int surfaceId = geometry.DefineParametricSurfaceExt(surfaceName, outerVertices, type, autoGenerate);
            geometry.AddPolygonalRegionToSurfaceExt(surfaceId, innerVertices, RegionType.Opening);

            return surfaceId;
        }

        /// <summary>
        /// Defines a parametric surface using the polygon points (vertices) provided.
        /// </summary>
        /// <param name="geometry"><see cref="OSGeometryUI"/> COM object to carry out geometry operations in Staad.</param>
        /// <param name="surfaceName">Name of the parametric surface to be created.</param>
        /// <param name="vertices">All vertices on the polygon. These nodes must exist in the model already.</param>
        /// <param name="type">Type of the surface being created. Refer <see cref="SurfaceType"/>.</param>
        /// <param name="autoGenerate"></param>
        /// <returns>An integer id of the created surface. A value of -1 indicates the surface is not created.</returns>
        public static int DefineParametricSurfaceExt(this OSGeometryUI geometry, string surfaceName, IEnumerable<Node> vertices,
            SurfaceType type = SurfaceType.None, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            List<Node> verticesList = vertices.ToList();
            Node startsAt = verticesList[0];
            Node xVertex = verticesList[1];
            Node yVertex = verticesList[2];
            return geometry.DefineParametricSurfaceExt(surfaceName, startsAt, xVertex, yVertex, vertices, type, autoGenerate);
        }

        /// <summary>
        /// Defines a parametric surface using the polygon points (vertices) provided.
        /// </summary>
        /// <param name="geometry"><see cref="OSGeometryUI"/> COM object to carry out geometry operations in Staad.</param>
        /// <param name="surfaceName">Name of the parametric surface to be created.</param>
        /// <param name="startsAt">Start <see cref="Node"/> of the boundary polygon of the surface.</param>
        /// <param name="xVertex">Vertex on the x-axis of the surface. Typically the second <see cref="Node"/> on the polygon.</param>
        /// <param name="yVertex">Vertex on the y-axis of the surface. Typically the third <see cref="Node"/> on the polygon.</param>
        /// <param name="vertices">All vertices on the polygon. These nodes must exist in the model already.</param>
        /// <param name="type">Type of the surface being created. Refer <see cref="SurfaceType"/>.</param>
        /// <param name="autoGenerate"></param>
        /// <returns>An integer id of the created surface. A value of -1 indicates the surface is not created.</returns>
        public static int DefineParametricSurfaceExt(this OSGeometryUI geometry, string surfaceName, Node startsAt, Node xVertex,
            Node yVertex, IEnumerable<Node> vertices,
            SurfaceType type = SurfaceType.None, AutoGenerate autoGenerate = AutoGenerate.No)
        {
            int nVertices = vertices.Count();
            int[] verticesIds = vertices.Select(v => v.Id).ToArray();

            return geometry.DefineParametricSurface(surfaceName, type, startsAt.Id, xVertex.Id, yVertex.Id, nVertices, verticesIds, autoGenerate);
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

            int bpSurfaceId = geometry.DefineParametricSurfaceExt(surfaceName, nodesList[0], nodesList[1], nodesList[2], nodes);
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

            int bpSurfaceId = geometry.DefineParametricSurfaceExt(surfaceName, nodesList[0], nodesList[1], nodesList[2], nodes);

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

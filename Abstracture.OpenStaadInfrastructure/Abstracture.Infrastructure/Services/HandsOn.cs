using OpenSTAADUI;
using System.Collections.Generic;

namespace Abstracture.Infrastructure.Services
{
    public static class HandsOn
    {
        #region WIP
        private static string _fillFullPath = @"C:\Users\srini\Desktop\On Screen\Coding\Staad\OpenStaad Example.std";

        public static void RootFunctions()
        {
            OpenSTAAD openStaad = OpenStaadService.GetInstance();
            var isSilentMode = openStaad.SetSilentMode(true);

            openStaad.OpenSTAADFile(_fillFullPath);

        }
        #endregion

        public static void GeometryFuncs()
        {
            OpenSTAAD instance = OpenStaadService.Instance();
            OSGeometryUI geometry = instance.Geometry;


            geometry.CreateNode(1, 0, 0, 0);
            geometry.CreateNode(2, 6, 0, 0);
            geometry.CreateNode(3, 12, 0, 0);



        }






























        #region Helpers

        public struct NodeCoordinates
        {
            public NodeCoordinates(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public double X { get; set; }

            public double Y { get; set; }

            public double Z { get; set; }

        }

        #endregion

        #region Backup

        public static void GeometryFuncys()
        {
            OpenSTAAD instance = OpenStaadService.Instance();

            OSGeometryUI geometry = instance.Geometry;
            OSLoadUI load = instance.Load;

            /// Nodes:

            geometry.CreateNode(1, 0, 0, 0);



            dynamic nodeId = geometry.AddNode(5, 5, 5);


            double[,] coords = new double[6, 3];

            coords[0, 0] = 0;
            coords[0, 1] = 0;
            coords[0, 2] = 0;

            coords[1, 0] = 5;
            coords[1, 1] = 0;
            coords[1, 2] = 0;

            coords[2, 0] = 10;
            coords[2, 1] = 0;
            coords[2, 2] = 0;

            coords[3, 0] = 0;
            coords[3, 1] = 5;
            coords[3, 2] = 0;

            coords[4, 0] = 5;
            coords[4, 1] = 5;
            coords[4, 2] = 0;

            coords[5, 0] = 10;
            coords[5, 1] = 5;
            coords[5, 2] = 0;



            List<NodeCoordinates> coordinates = new List<NodeCoordinates>()
            {
                new NodeCoordinates(0, 0, 0),
                new NodeCoordinates(5, 0, 0),
                new NodeCoordinates(10, 0, 0),
                new NodeCoordinates(0, 5, 0),
                new NodeCoordinates(5, 5, 0),
                new NodeCoordinates(10, 5, 0)
            };

            double[,] osCoords = new double[coordinates.Count, 3];

            for (int i = 0; i < coordinates.Count; i++)
            {
                osCoords[i, 0] = coordinates[i].X;
                osCoords[i, 1] = coordinates[i].Y;
                osCoords[i, 2] = coordinates[i].Z;
            }

            /// Adds multiple nodes based on the coordinates array provided.
            geometry.AddMultipleNodes(osCoords);

            /// Retrieves the last node number used in the current model.
            dynamic lastNodeNumber = geometry.GetLastNodeNo();

            /// Retrieves the total nodes count from the current model.
            dynamic nodeCount = geometry.GetNodeCount();

            /// Gets the node number for a node by matching the specified coordinates
            /// Note: Returns '0' if the coordinates does not match with if any of the nodes.
            dynamic nodeNumber = geometry.GetNodeNumber(5, 0, 0);

            /// Gets the distance between two nodes specified.
            /// Note: Returns '0' if one or both of the nodes specified does not exist in the current model.
            dynamic nodeDistance = geometry.GetNodeDistance(15, 8);

            geometry.DeleteNode(5);
            geometry.DeleteNode(6);
            geometry.DeleteNode(7);



        }

        #endregion

    }
}

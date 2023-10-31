using Newtonsoft.Json;

using OpenSTAADUI;

using ReInvented.Shared;
using ReInvented.Shared.Services;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SPro2023ConsoleApp
{
    

    public class Polygon
    {

        public Polygon()
        {
            Vertices = new List<Node>();
        }

        public List<Node> Vertices { get; set; }

        public double GetArea()
        {
            List<Node> vertices = Vertices.ToList();

            double sum1 = 0.0;
            double sum2 = 0.0;

            for (int i = 0; i < vertices.Count(); i++)
            {
                Node currentNode = vertices[i];
                Node nextNode = i == vertices.Count() - 1 ? vertices[0] : vertices[i + 1];
                sum1 += currentNode.X * nextNode.Y;
                sum2 += currentNode.Y * nextNode.X;
            }

            return 0.50 * Math.Abs(sum1 - sum2);
        }

        public double GetArea3D()
        {
            List<Node> vertices = Vertices.ToList();

            Vector3D v1 = vertices[1].ToVector - vertices[0].ToVector;
            Vector3D v2 = vertices[2].ToVector - vertices[0].ToVector;
            Vector3D v3 = vertices[3].ToVector - vertices[0].ToVector;

            Vector3D normal1 = Vector3D.CrossProduct(v1, v2);
            Vector3D normal2 = Vector3D.CrossProduct(v1, v3);

            double area1 = 0.5 * normal1.Length;
            double area2 = 0.5 * normal2.Length;

            double totalArea = area1 + area2;

            return totalArea;
        }

    }




    class Program
    {
        static void Main(string[] args)
        {
            ///await Previous();

            StaadModel model = new StaadModel();
            StaadModelWrapper wrapper = model.StaadWrapper;

            ImmediateTakeOff.GeneratePlatesMtoTableInStaadModel(wrapper);

            OSGeometryUI geometry = wrapper.StaadInstance.Geometry;
            OSPropertyUI property = wrapper.StaadInstance.Property;

            OSOutputUI output = wrapper.StaadInstance.Output;

            if (output.AreResultsAvailable())
            {
                int nodeNumber = 16925;
                object reactions = new double[6];
                output.GetSupportReactions(nodeNumber, 601, ref reactions);
            }


            var staadProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.StartsWith("Bentley") && p.ProcessName.EndsWith("Staad"));
            var staadWindowHandle = staadProcess.MainWindowHandle;

            WindowServices.MinimizeWindow(staadWindowHandle);
            WindowServices.RestoreWindow(staadWindowHandle);
            WindowServices.HideWindow(staadWindowHandle);
            WindowServices.MaximizeWindow(staadWindowHandle);
        }

        private static async Task Previous()
        {
            string fileFullPath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\0D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250.std";

            StaadWrapper wrapper = new StaadWrapper();
            OSGeometryUI geometry = wrapper.Geometry;


            NodesCollection nodesCollection = ReadNodesFromJson();

            int threadCount = 360;

            if (nodesCollection.Nodes.Count / threadCount >= 1)
            {
                var nodeCountInEachThread = (int)(nodesCollection.Nodes.Count / threadCount).Floor(1);
                var threadNodesCollections = new List<List<Node>>();

                var tracker = 0;

                do
                {
                    threadNodesCollections.Add(new List<Node>(nodesCollection.Nodes.Skip(tracker * nodeCountInEachThread).Take(nodeCountInEachThread)));
                    tracker++;

                } while (tracker * nodeCountInEachThread < nodesCollection.Nodes.Count());

                await Task.Run(() => wrapper.OpenStaad.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN));
                Thread.Sleep(5000);

                List<Task> tasks = threadNodesCollections.Select(tnc => CreateNodesOneByOne(geometry, tnc)).ToList();

                await Task.WhenAll(tasks);
            }
            else
            {
                wrapper.OpenStaad.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN);
                Thread.Sleep(5000);

                await Task.Run(() => CreateNodesOneByOne(geometry, nodesCollection.Nodes.ToList()));
            }

            ///// Case 1:
            //CreateNodesOneByOne(geometry, nodesCollection.Nodes.ToList());
            ///// Case 2a:
            /////CreateNodesAllAtOnceUsingArrayTypeOne(geometry, nodesCollection.Nodes.ToList());
            ///// Case 2b:
            /////CreateNodesAllAtOnceUsingArrayTypeTwo(geometry, nodesCollection.Nodes.ToList());
            //wrapper.OpenStaad.SaveModel(1);
        }

        private static async Task CreateNodesOneByOne(OSGeometryUI geometry, List<Node> nodes)
        {
            await Task.Run(() => nodes.ForEach(n => geometry.CreateNode(n.Id, n.X, n.Y, n.Z)));
        }

        private static void CreateNodesAllAtOnceUsingArrayTypeOne(OSGeometryUI geometry, List<Node> nodes)
        {
            int[] nodeIds = nodes.Select(n => n.Id).ToArray();
            double[,] nodeCoords = new double[nodeIds.Count(), 3];

            for (int i = 0; i < nodes.Count; i++)
            {
                nodeCoords[i, 0] = nodes[i].X;
                nodeCoords[i, 1] = nodes[i].Y;
                nodeCoords[i, 2] = nodes[i].Z;
            }
            geometry.CreateMultipleNodes(nodeIds, nodeCoords);
        }

        private static void CreateNodesAllAtOnceUsingArrayTypeTwo(OSGeometryUI geometry, List<Node> nodes)
        {
            int[] nodeIds = nodes.Select(n => n.Id).ToArray();
            double[][] nodeCoords = nodes.Select(n => new double[] { n.X, n.Y, n.Z }).ToArray();

            geometry.CreateMultipleNodes(nodeIds, nodeCoords);
        }

        private static NodesCollection ReadNodesFromJson()
        {
            string fileContents = File.ReadAllText("NodesCollection.json");
            NodesCollection deserialized = JsonConvert.DeserializeObject<NodesCollection>(fileContents);

            return deserialized;
        }
    }
}

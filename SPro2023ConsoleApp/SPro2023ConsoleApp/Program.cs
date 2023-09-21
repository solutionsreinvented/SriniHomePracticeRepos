using Newtonsoft.Json;

using OpenSTAADUI;

using ReInvented.Shared;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SPro2023ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string fileFullPath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\0D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250.std";

            StaadWrapper wrapper = new StaadWrapper();
            OSGeometryUI geometry = wrapper.Geometry;

            NodesCollection nodesCollection = ReadNodesFromJson();

            int threadCount = 12;

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

using Newtonsoft.Json;

using OpenSTAADUI;

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Domain.Reporting.Services;
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

namespace SPro2023ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ///await Previous();

            StaadModel model = new StaadModel();
            StaadModelWrapper wrapper = model.StaadWrapper;

            Report<FoundationLoadData> foundationLoadData = new Report<FoundationLoadData>();
            Report<MaterialTakeOff> materialTakeOff = new Report<MaterialTakeOff>();


            DataSourceInformation sourceInfo = new DataSourceInformation()
            {
                PreparedOn = DateTime.Now.ToString("F")
            };

            materialTakeOff.Content = MaterialTakeOffService.Generate(wrapper, sourceInfo);

            //MaterialTakeOff mto = MaterialTakeOffService.Generate(wrapper, sourceInfo);

            //double totalWeight = mto.TotalWeight;

            OSGeometryUI geometry = wrapper.StaadInstance.Geometry;
            OSPropertyUI property = wrapper.StaadInstance.Property;

            OSOutputUI output = wrapper.StaadInstance.Output;

            if (output.AreResultsAvailable())
            {
                int nodeNumber = 16925;
                object reactions = new double[6];
                output.GetSupportReactions(nodeNumber, 601, ref reactions);
            }


            Process staadProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.StartsWith("Bentley") && p.ProcessName.EndsWith("Staad"));
            IntPtr staadWindowHandle = staadProcess.MainWindowHandle;

            WindowServices.MinimizeWindow(staadWindowHandle);
            WindowServices.RestoreWindow(staadWindowHandle);
            WindowServices.HideWindow(staadWindowHandle);
            WindowServices.MaximizeWindow(staadWindowHandle);
        }

        private static async Task Previous()
        {
            string fileFullPath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\0D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250.std";

            StaadModel model = new StaadModel();
            StaadModelWrapper wrapper = model.StaadWrapper;
            OpenSTAAD instance = wrapper.StaadInstance;
            OSGeometryUI geometry = instance.Geometry;


            NodesCollection nodesCollection = ReadNodesFromJson();

            int threadCount = 360;

            if (nodesCollection.Nodes.Count / threadCount >= 1)
            {
                int nodeCountInEachThread = (int)(nodesCollection.Nodes.Count / threadCount).Floor(1);
                List<List<Node>> threadNodesCollections = new List<List<Node>>();

                int tracker = 0;

                do
                {
                    threadNodesCollections.Add(new List<Node>(nodesCollection.Nodes.Skip(tracker * nodeCountInEachThread).Take(nodeCountInEachThread)));
                    tracker++;

                } while (tracker * nodeCountInEachThread < nodesCollection.Nodes.Count());

                await Task.Run(() => instance.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN));
                Thread.Sleep(5000);

                List<Task> tasks = threadNodesCollections.Select(tnc => CreateNodesOneByOne(geometry, tnc)).ToList();

                await Task.WhenAll(tasks);
            }
            else
            {
                instance.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN);
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

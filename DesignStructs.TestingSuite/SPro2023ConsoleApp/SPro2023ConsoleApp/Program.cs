
using Newtonsoft.Json;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ReInvented.StaadPro.Interactivity.Extensions;
using SPro2023ConsoleApp.Models;
using ReInvented.StaadPro.Interactivity.Enums;

namespace SPro2023ConsoleApp
{


    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

    }

    class Program
    {

        static void Main(string[] args)
        {
            var text = "1 16777 16825 16826 16778; 2 16778 16826 16827 16778;";

            var plates = StaadEditorFile.RectifyPlateIncidences(text);


            string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250_Staad Editor Testing.std";
            StaadEditorFile staadFile = new StaadEditorFile(filePath, StaadEditorSettings.GetDefaultSettings());

            staadFile.FormatEntityIncidences(EntityType.Nodes);

            var (StartIndex, EndIndex) = StaadEditorFile.GetEntityIncidencesRange(staadFile, EntityType.Plates);


            ///await Previous();

            StaadModel model = new StaadModel();
            StaadModelWrapper wrapper = model.StaadWrapper;

            OSGeometryUI geometry = wrapper.Geometry as OSGeometryUI;

            _ = staadFile.RemoveFilePath()
                         .AddCopyRight(new List<string>())
                         .AddBeforeLineContaining("", new List<string>())
                         .AddAfterLineContaining("", new List<string>())
                         .FixFourNodedTriangularPlate(new Plate())
                         .FixTriangularPlatesIncidences(new List<Plate>())
                         .FixSyntaxForTriangularPlates();

            var allPlates = geometry.GetAllEntitiesOfType<Plate>();

            var triangularPlates = allPlates.Where(p => p.D == null || p.A == p.D);

            foreach (var tp in triangularPlates)
            {
                geometry.DeletePlate(tp.Id);
                geometry.CreatePlate(tp.Id, tp.A.Id, tp.C.Id, tp.B.Id, tp.A.Id);
            }


            //DataSource sourceInfo = new DataSource()
            //{
            //    PreparedOn = DateTime.Now.ToString("F")
            //};

            ////MaterialTakeOff mto = MaterialTakeOffService.Generate(wrapper, sourceInfo);

            ////double totalWeight = mto.TotalWeight;

            //OSGeometryUI geometry = wrapper.StaadInstance.Geometry;
            //OSPropertyUI property = wrapper.StaadInstance.Property;

            //OSOutputUI output = wrapper.StaadInstance.Output;

            //if (output.AreResultsAvailable())
            //{
            //    int nodeNumber = 16925;
            //    object reactions = new double[6];
            //    output.GetSupportReactions(nodeNumber, 601, ref reactions);
            //}


            //Process staadProcess = Process.GetProcesses().FirstOrDefault(p => p.ProcessName.StartsWith("Bentley") && p.ProcessName.EndsWith("Staad"));
            //IntPtr staadWindowHandle = staadProcess.MainWindowHandle;

            //WindowServices.MinimizeWindow(staadWindowHandle);
            //WindowServices.RestoreWindow(staadWindowHandle);
            //WindowServices.HideWindow(staadWindowHandle);
            //WindowServices.MaximizeWindow(staadWindowHandle);
        }



        //private static async Task Previous()
        //{
        //    string fileFullPath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\0D35.0H2.50S09.00OC1.113SC1.219IMP0.240CON0.0300MOT1250.std";

        //    StaadModel model = new StaadModel();
        //    StaadModelWrapper wrapper = model.StaadWrapper;
        //    OpenSTAAD instance = wrapper.StaadInstance;
        //    OSGeometryUI geometry = instance.Geometry;


        //    NodesCollection nodesCollection = ReadNodesFromJson();

        //    int threadCount = 360;

        //    if (nodesCollection.Nodes.Count / threadCount >= 1)
        //    {
        //        int nodeCountInEachThread = (int)(nodesCollection.Nodes.Count / threadCount).Floor(1);
        //        List<List<Node>> threadNodesCollections = new List<List<Node>>();

        //        int tracker = 0;

        //        do
        //        {
        //            threadNodesCollections.Add(new List<Node>(nodesCollection.Nodes.Skip(tracker * nodeCountInEachThread).Take(nodeCountInEachThread)));
        //            tracker++;

        //        } while (tracker * nodeCountInEachThread < nodesCollection.Nodes.Count());

        //        await Task.Run(() => instance.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN));
        //        Thread.Sleep(5000);

        //        //List<Task> tasks = threadNodesCollections.Select(tnc => CreateNodesOneByOne(geometry, tnc)).ToList();

        //        await Task.WhenAll(tasks);
        //    }
        //    else
        //    {
        //        instance.NewSTAADFile(fileFullPath, LengthUnit.M, ForceUnit.KN);
        //        Thread.Sleep(5000);

        //        await Task.Run(() => CreateNodesOneByOne(geometry, nodesCollection.Nodes.ToList()));
        //    }

        //    ///// Case 1:
        //    //CreateNodesOneByOne(geometry, nodesCollection.Nodes.ToList());
        //    ///// Case 2a:
        //    /////CreateNodesAllAtOnceUsingArrayTypeOne(geometry, nodesCollection.Nodes.ToList());
        //    ///// Case 2b:
        //    /////CreateNodesAllAtOnceUsingArrayTypeTwo(geometry, nodesCollection.Nodes.ToList());
        //    //wrapper.OpenStaad.SaveModel(1);
        //}

        private static NodesCollection ReadNodesFromJson()
        {
            string fileContents = File.ReadAllText("NodesCollection.json");
            NodesCollection deserialized = JsonConvert.DeserializeObject<NodesCollection>(fileContents);

            return deserialized;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

using DevDrive.Services;

using OpenSTAADUI;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace DevDrive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //var result = ApplicationServices.StartApplication(ApplicationEdition.Connect.GetDescription(), "Staad", 60);

            MaterialsLibrary matLib = MaterialsRepository.Instance.GetMaterialsLibrary();

            IEnumerable<MaterialGrade> allGrades = matLib.Tables.SelectMany(t => t.Grades);
            IEnumerable<MaterialGrade> matched = allGrades.Where(g => g.Designation.Contains("A36"));

            MaterialGrade grade = matched.FirstOrDefault(g => g.StaadName == "A36");

            OptimizePlates(grade);
        }

        #region Future Use Functions
        private static void OptimizePlates(MaterialGrade materialGrade)
        {
            var nThreads = 30;

            string filePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad Models", "*.std"));
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string outputFilePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_PlatesOptimization.res");
            string outputJsonFilePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_PlatesOptimization.js");

            OpenStaadWrapper wrapper = OSGlobalExtensions.GetOpenStaadWrapper(filePath);

            //Testing Region

            OSOutputUI output = wrapper.Output as OSOutputUI;
            OSGeometryUI geometry = wrapper.Geometry as OSGeometryUI;
            OSLoadUI load = wrapper.Load as OSLoadUI;

            IEnumerable<Plate> allPlates = geometry.GetAllEntities<Plate>(nThreads);
            HashSet<LoadCase> plc = load.GetAllPrimaryLoadCases();

            //var plateCenterResults = output.GetPlateCenterResults(plc, allPlates, 5);

            //End Region

            List<string> groupNames = (wrapper.Geometry as OSGeometryUI)
                                      .GetEntityGroups<Plate>(nThreads)
                                      .Where(eg => eg.Entities.Count() > 0)
                                      .OrderByDescending(eg => Plate.MaxYCoordinate(eg.Entities.OrderByDescending(e => Plate.MaxYCoordinate(e)).First()))
                                      .Where(eg => !InExclusionList(eg.GroupName))
                                      .Select(eg => eg.GroupName).ToList();

            double limitingStress = 0.9 * materialGrade.Fy;

            int sLcId = 101;
            int eLcId = 200;

            double minimumThickness = 6.0;
            double corrosionAllowance = 2.0;

            IEnumerable<LoadCase> loadCases = (wrapper.Load as OSLoadUI).GetLoadCasesFromIds(Enumerable.Range(sLcId, eLcId - sLcId + 1), LoadCaseType.LoadCombination);
            PlatesOptimizationService pos = new PlatesOptimizationService(wrapper, materialGrade);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<PlateGroupDesignResult> designResults = pos.OptimizePlateGroups(groupNames, minimumThickness, corrosionAllowance, loadCases, 10.0 );

            stopwatch.Stop();
            var elapsed = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

            Console.WriteLine($"Total time consumed for optimizing the plates is {elapsed.Minutes} minutes, {elapsed.Seconds} seconds and {elapsed.Milliseconds}");

            JsonDataSerializer<List<PlateGroupDesignResult>> serializer = new JsonDataSerializer<List<PlateGroupDesignResult>>();
            var serialized = "const content = " + serializer.Serialize(designResults, JsonSerializerSettingsProvider.Minified);

            File.WriteAllText(outputJsonFilePath, serialized);
        }

        private static bool InExclusionList(string groupName)
        {
            return groupName.Contains("TANK") || groupName.Contains("COMP") || groupName.Contains("CENTRE") || groupName.Contains("LAUNDER");
        }


        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

using DevDrive.LogWorks;
using DevDrive.Services;

using Microsoft.Extensions.Logging;

using OpenSTAADUI;

using ReInvented.Domain.Optimization.Models;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared.Interfaces;
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
            string fullPath = @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\Demo\Steady State\03. STAAD\01. Working\D46.0H3.00S09.00OC1.129SC1.247IMP0.155CON0.0053MOT1033.STD";
            StaadModel model = new StaadModel(fullPath);
            OpenStaadWrapper wrapper = model.OpenStaadWrapper;
            Marshal.ReleaseComObject(wrapper.Design);

            //IResult<string> result = ApplicationServices.StartApplication(@"C:\Program Files\Bentley\Engineering\STAAD.Pro 2023\STAAD\Bentley.Staad.exe", "STAAD.Pro", 60);

            MainWindow = new MainWindow();
            MainWindow.Show();

            //string inputFile = @"D:\02. Due\00. Projects\01. Pre-Order\72. E24080555 (90m JSW)\07. References\Load Combinations\IS800-2007LSD.std";
            //HashSet<Envelop> envelops = new HashSet<Envelop>()
            //{
            //    new Envelop(EnvelopGroup.JointMass, 100, 100),
            //    new Envelop(EnvelopGroup.Strength, 101, 204),
            //    new Envelop(EnvelopGroup.Serviceability, 1001, 1040)
            //};

            //var combs = LoadCombinationsDefinition.Parse(inputFile, "IS800", 2007, "HRT", "Standard", envelops);
            //LoadCombinationsDefinition.PersistAsTemplate(combs, Path.GetDirectoryName(inputFile));

            //Stopwatch stopwatch = new Stopwatch();
            //MaterialGrade grade = MaterialsRepository.Instance
            //                                         .GetMaterialsLibrary().Tables
            //                                         .SelectMany(t => t.Grades)
            //                                         .Where(g => g.Designation.Contains("A36"))
            //                                         .FirstOrDefault(g => g.StaadName == "A36");

            //OpenStaadWrapper wrapper = OpenStaadWrapper.GetStaadWrapper();
            //IEnumerable<LoadCase> loadCases = (wrapper.Load as OSLoadUI).GetLoadCases(101, 200, LoadCaseType.LoadCombination);
            //var criteria = new PlateOptimizationCriteria()
            //{ MinimumThickness = 6.0, CorrosionAllowance = 2.0, MaterialGrade = grade, ThreadCount = 30, AllowedPercentPlatesExceedance = 15.0 };

            //stopwatch.Start();

            //PlatesOptimizationService pos = new PlatesOptimizationService(wrapper, criteria);
            //HashSet<PlateGroupDesignResult> results = pos.OptimizeAll(loadCases);

            //stopwatch.Stop();
            //Console.WriteLine($"Total time consumed for plates optimization is {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)}");

            //pos.WriteResultsToFile(results);
        }

        #region Previous - Successful

        private void OptimizePlates(MaterialGrade materialGrade, int nThreads)
        {

            //string filePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad Models", "*.std"));
            //string directory = Path.GetDirectoryName(filePath);
            //string fileName = Path.GetFileName(filePath);
            //string outputFilePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_PlatesOptimization.res");
            //string outputJsonFilePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_PlatesOptimization.js");

            //OpenStaadWrapper wrapper = OSGlobalExtensions.GetOpenStaadWrapper(filePath);

            ////Testing Region

            //OSOutputUI output = wrapper.Output as OSOutputUI;
            //OSGeometryUI geometry = wrapper.Geometry as OSGeometryUI;
            //OSLoadUI load = wrapper.Load as OSLoadUI;

            //IEnumerable<Plate> allPlates = geometry.GetAllEntities<Plate>(nThreads);
            //HashSet<LoadCase> plc = load.GetAllPrimaryLoadCases();

            ////End Region

            //List<string> groupNames = geometry.GetEntityGroups<Plate>(nThreads)
            //                          .Where(eg => eg.Entities.Count() > 0)
            //                          .OrderByDescending(eg => Plate.MaxYCoordinate(eg.Entities.OrderByDescending(e => Plate.MaxYCoordinate(e)).First()))
            //                          .Where(eg => !InExclusionList(eg.GroupName))
            //                          .Select(eg => eg.GroupName).ToList();

            //int sLcId = 101;
            //int eLcId = 200;

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            //IEnumerable<LoadCase> loadCases = (wrapper.Load as OSLoadUI).GetLoadCasesFromIds(Enumerable.Range(sLcId, eLcId - sLcId + 1), LoadCaseType.LoadCombination);
            //var criteria = new PlateOptimizationCriteria() { MinimumThickness = 6.0, CorrosionAllowance = 2.0, MaterialGrade = materialGrade, ThreadCount = nThreads, AllowedPercentPlatesExceedance = 15.0 };

            //PlatesOptimizationService pos = new PlatesOptimizationService(wrapper, criteria);
            //List<PlateGroupDesignResult> designResults = pos.OptimizePlateGroups(groupNames, loadCases);

            //stopwatch.Stop();
            //TimeSpan elapsed = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);

            //Console.WriteLine($"Total time consumed for optimizing the plates is {elapsed.Minutes} minutes, {elapsed.Seconds} seconds and {elapsed.Milliseconds}");

            //pos.WriteResultsToFile(designResults);
        }

        #endregion

    }
}

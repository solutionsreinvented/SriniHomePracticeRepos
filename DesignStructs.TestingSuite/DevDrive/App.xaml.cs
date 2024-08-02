using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

using DevDrive.Models;
using DevDrive.Services;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.Units.Models;

namespace DevDrive
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            string filePath = @"D:\02. Due\00. Projects\01. Pre-Order\67. E24070486 (Serra Verde) 55m\03. STAAD\01. Working\D55.0H5.00S14.00OC1.179SC1.353IMP0.007CON0.0002MOT4000.std";
            string outputFilePath = @"D:\02. Due\00. Projects\01. Pre-Order\67. E24070486 (Serra Verde) 55m\03. STAAD\01. Working\D55.0H5.00S14.00OC1.179SC1.353IMP0.007CON0.0002MOT4000_DesignThicknesses.res";
            //List<string> groupNames = new List<string>
            //{
            //    "_FP_PCD1", "_FP_PCD2", "_FP_PCD3", "_FP_PCD4", "_FP_PCD5", "_FP_PCD6",
            //    "_WALL", "_WALL_DROOP", "_WALL_LW", "_LWALL", "_LFLOOR",
            //    "_CC_TOP", "_CC_BOTTOM", "_UF_CONE"
            //};

            List<string> groupNames = new List<string>
            {
                "_FP_PCD1", "_LWALL", "_LFLOOR", "_UF_CONE", "_WALL_DROOP"
            };

            OpenStaadWrapper wrapper = OSGlobalExtensions.GetOpenStaadWrapper(filePath);

            double limitingStress = 240.0;

            int sLcId = 101;
            int eLcId = 200;

            IEnumerable<LoadCase> loadCases = (wrapper.Load as OSLoadUI).GetLoadCasesFromIds(Enumerable.Range(sLcId, eLcId - sLcId + 1), LoadCaseType.LoadCombination);
            PlatesOptimizationService pos = new PlatesOptimizationService(wrapper, limitingStress);

            Dictionary<string, PlateGroupDesignResult> designResults = pos.OptimizePlateGroups(groupNames, loadCases, 30.0);


            List<string> resultContent = new List<string>() { "------------------------------ Summary of Results -----------------------------" };
            string separator = $"  |  ";
            resultContent.Add($"{separator}{"Group Name",-12}{separator}{"Design Thickness",-20}{separator}{"Absolute Max Von Mises",-25}{separator}{"Percentage of Plates Exceeding",29}{separator}");
            resultContent.AddRange(designResults.Select(dt => TransformResult(separator, dt)));
            resultContent.Add("------------------------------ End of Results -----------------------------");
            resultContent.Add(Environment.NewLine);

            File.AppendAllLines(outputFilePath, resultContent);
        }

        private static string TransformResult(string separator, KeyValuePair<string, PlateGroupDesignResult> dt)
        {
            return $"{separator}{dt.Key,-12}{separator}{dt.Value.Thickness,-20:N2}{separator}" +
                   $"{dt.Value.StressSummary.GoverningResults.VonMises.AbsoluteMaximum/1000,-25:N2}{separator}" +
                   $"{dt.Value.StressSummary.PercentPlatesExceeding,29:N2}%{separator}";
        }

    }
}

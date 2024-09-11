using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using DevDrive.Services;

using OpenSTAADUI;

using ReInvented.DataAccess.Models;
using ReInvented.DataAccess.Services;
using ReInvented.Shared;
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
            OptimizePlates();
        }

        #region Future Use Functions
        private static void OptimizePlates()
        {
            string filePath = FileServiceProvider.GetFilePathUsingOpenFileDialog(new FileFilter("Staad Models", "*.std"));
            string directory = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string outputFilePath = Path.Combine(directory, $"{Path.GetFileNameWithoutExtension(fileName)}_DesignThicknesses.res");

            OpenStaadWrapper wrapper = OSGlobalExtensions.GetOpenStaadWrapper(filePath);

            List<string> groupNames = (wrapper.Geometry as OSGeometryUI)
                                      .GetEntityGroupsOfType<Plate>()
                                      .OrderByDescending(eg => Plate.MaxYCoordinate(eg.Entities.OrderByDescending(e => Plate.MaxYCoordinate(e)).First()))
                                      .Where(eg => !InExclusionList(eg.GroupName))
                                      .Select(eg => eg.GroupName).ToList();

            double limitingStress = 315.0;

            int sLcId = 101;
            int eLcId = 200;

            double minimumThickness = 6.0;
            double corrosionAllowance = 0.0;

            IEnumerable<LoadCase> loadCases = (wrapper.Load as OSLoadUI).GetLoadCasesFromIds(Enumerable.Range(sLcId, eLcId - sLcId + 1), LoadCaseType.LoadCombination);
            PlatesOptimizationService pos = new PlatesOptimizationService(wrapper, limitingStress);

            Dictionary<string, PlateGroupDesignResult> designResults = pos.OptimizePlateGroups(groupNames, minimumThickness, corrosionAllowance, loadCases, 10.0);


            List<string> resultContent = new List<string>() { $"{Pad(Header, MaxLength)} {Header} {Pad(Header, MaxLength)}" };
            string separator = $"  |  ";
            resultContent.Add($"{separator}{"Group Name",-15}{separator}{"Design Thickness",-20}{separator}{"Max Von Mises",-20}{separator}{"L/C",-10}{separator}{"% Plates Exceeding",20}{separator}");
            resultContent.AddRange(designResults.Select(dt => TransformResult(separator, dt)));
            resultContent.Add($"{Pad(Footer, MaxLength)} {Footer} {Pad(Footer, MaxLength)}");
            resultContent.Add(Environment.NewLine);

            File.AppendAllLines(outputFilePath, resultContent);
        }

        private static bool InExclusionList(string groupName)
        {
            return groupName.Contains("TANK") || groupName.Contains("COMP") || groupName.Contains("CENTRE") || groupName.Contains("LAUNDER");
        }

        private static string TransformResult(string separator, KeyValuePair<string, PlateGroupDesignResult> dt)
        {
            return $"{separator}{dt.Key,-15}{separator}{dt.Value.Thickness,-20:N2}{separator}" +
                   $"{dt.Value.StressSummary.GoverningResults.VonMises.AbsoluteMaximum / 1000,-20:N2}{separator}" +
                   $"{dt.Value.StressSummary.GoverningResults.LoadCase.Id,-10}{separator}" +
                   $"{dt.Value.StressSummary.PercentPlatesExceeding,20:N2}%{separator}";
        }

        private static int MaxLength => 85;

        private static string Header => "Summary of Results";
        private static string Footer => "End of Results";

        private static string Pad(string content, int maxLength) => string.Join("", Enumerable.Repeat("-", Padding(content, maxLength)));

        private static int Padding(string content, int maxLength) => ((maxLength - content.Length) / 2).Ceiling(1);

        #endregion

    }
}

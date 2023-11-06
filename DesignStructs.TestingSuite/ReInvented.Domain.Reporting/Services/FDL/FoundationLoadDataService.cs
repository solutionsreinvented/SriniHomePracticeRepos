using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using HtmlAgilityPack;

using OpenSTAADUI;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Geometry.Models;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Extensions;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;
using ReInvented.StaadPro.Interactivity.Services.Staad;

namespace ReInvented.Domain.Reporting.Services
{
    public class FoundationLoadDataService
    {
        #region Public Functions

        public static FoundationLoadData Generate(IProjectInfo projectInfo, IEnumerable<int> reportLoadsIds)
        {
            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            OSOutputUI output = instance.Output;

            HashSet<Node> nodes = StaadGeometryServices.GetAllNodes(instance.Geometry);
            HashSet<EntityGroup> pcdSupportGroups = GetSupportEntityGroups(instance.Geometry);
            List<LoadCase> loadCases = GetPrimaryLoadCasesForLoadDataGeneration(instance.Load, reportLoadsIds);

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, nodes, loadCases, projectInfo, model.ModelName);

            return foundationLoadData;
        }

        public static FoundationLoadData Generate(StaadModel model, IProjectInfo projectInfo, IEnumerable<int> reportLoadsIds)
        {
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<EntityGroup> pcdSupportGroups = model.EntityGroups.Where(g => g.GroupName.Contains("_SUP_") && g.EntityType == EntityType.Nodes).ToHashSet();
            List<LoadCase> loadCases = model.PrimaryLoads.Where(pl => reportLoadsIds.Contains(pl.Id)).OrderBy(lc => lc.Id).ToList();

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, model.Nodes, loadCases, projectInfo, model.FileName);

            return foundationLoadData;
        }

        #endregion

        #region Report Documents Generation

        public static void CreateReportHtmlFile(DirectoryInfo projectDirectory, bool useAbsolutePaths)
        {
            string fdlHtmlSourceFileFullPath = Path.Combine(FileServiceProvider.TemplatesDirectory, "Pages", ReportFileNames.HtmlFoundationLoadData);
            string fdlHtmlDestinationFileFullPath = Path.Combine(projectDirectory.FullName, ReportFileNames.HtmlFoundationLoadData);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.Load(fdlHtmlSourceFileFullPath);
            htmlDocument = htmlDocument.LinkCssAndScriptsTo(useAbsolutePaths);

            htmlDocument.Save(fdlHtmlDestinationFileFullPath);

        }

        public static void CopyCssStyleFiles(DirectoryInfo projectDirectory)
        {
            string sourceStylesDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Styles");
            string destinationStylesDirectory = Path.Combine(projectDirectory.FullName, "Styles");

            if (!Directory.Exists(destinationStylesDirectory))
            {
                _ = Directory.CreateDirectory(destinationStylesDirectory);
            }

            File.Copy(Path.Combine(sourceStylesDirectory, ReportFileNames.CssCommon), Path.Combine(destinationStylesDirectory, ReportFileNames.CssCommon), true);
            File.Copy(Path.Combine(sourceStylesDirectory, ReportFileNames.CssFoundationLoadData), Path.Combine(destinationStylesDirectory, ReportFileNames.CssFoundationLoadData), true);
        }

        public static void CopyJavaScriptFiles(DirectoryInfo projectDirectory)
        {
            string sourceScriptsDirectory = Path.Combine(FileServiceProvider.TemplatesDirectory, "Scripts");
            string destinationScriptsDirectory = Path.Combine(projectDirectory.FullName, "Scripts");

            if (!Directory.Exists(destinationScriptsDirectory))
            {
                _ = Directory.CreateDirectory(destinationScriptsDirectory);
            }

            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptCanvasGraphics), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptCanvasGraphics), true);
            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptSupportLayoutHelpers), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptSupportLayoutHelpers), true);
            File.Copy(Path.Combine(sourceScriptsDirectory, ReportFileNames.JavaScriptFoundationLoadData), Path.Combine(destinationScriptsDirectory, ReportFileNames.JavaScriptFoundationLoadData), true);
        }

        public static void CreateReportContentsFile(DirectoryInfo projectDirectory, FoundationLoadData foundationLoadData)
        {
            DirectoryInfo projectDataDirectory = Directory.CreateDirectory(Path.Combine(projectDirectory.FullName, "Data"));

            JsonDataSerializer<FoundationLoadData> serializer = new JsonDataSerializer<FoundationLoadData>();
            string seializedContent = "const FoundationLoadDataReport = " + serializer.Serialize(foundationLoadData, JsonSerializerSettingsProvider.MinifiedSettings());

            File.WriteAllText(Path.Combine(projectDataDirectory.FullName, ReportFileNames.ContentsFoundationLoadData), seializedContent);
        }

        #endregion

        #region Private Helpers - Core

        private static FoundationLoadData GenerateFoundationLoadData(OSOutputUI output, HashSet<EntityGroup> pcdSupportGroups, HashSet<Node> nodes,
                                                                     List<LoadCase> loadCases, IProjectInfo projectInfo, string staadFilename)
        {
            if (!output.AreResultsAvailable())
            {
                return null;
            }

            FoundationLoadData foundationLoadData = new FoundationLoadData();

            foundationLoadData.LoadCases = new Dictionary<int, string>();
            loadCases.ForEach(lc => foundationLoadData.LoadCases.Add(lc.Id, lc.Title));

            foundationLoadData.PCDLoadsCollection = RetrievePCDLoadsCollection(pcdSupportGroups, nodes, loadCases, output);

            foreach (PCDLoads pcd in foundationLoadData.PCDLoadsCollection)
            {
                pcd.SupportLoadsSummary = SupportLoadsSummaryService.Generate(pcd);
            }

            return foundationLoadData;
        }

        private static HashSet<PCDLoads> RetrievePCDLoadsCollection(HashSet<EntityGroup> pcdSupportGroups, HashSet<Node> nodes, List<LoadCase> loadCases, OSOutputUI output)
        {
            HashSet<PCDLoads> pcdLoadsCollection = new HashSet<PCDLoads>();

            foreach (EntityGroup psg in pcdSupportGroups.OrderBy(g => g.GroupName))
            {
                List<Node> supportNodes = nodes.Where(n => psg.Entities.Contains(n.Id)).ToList();
                PCDLoads pcdLoads = RetrievePCDLoadsFor(psg.GroupName, supportNodes, loadCases, output);

                _ = pcdLoadsCollection.Add(pcdLoads);

            }

            return pcdLoadsCollection;
        }

        private static PCDLoads RetrievePCDLoadsFor(string groupName, List<Node> supportNodes, List<LoadCase> loadCases, OSOutputUI output)
        {
            Node center = Circle.GetCenter(supportNodes);
            double diameter = Math.Round(Node.Diameter(supportNodes.First(), center), 3);

            PCDLoads pcdLoads = new PCDLoads()
            {
                PCD = groupName.Replace("_SUP_", ""),
                SupportsInformation = new SupportsInformation()
                {
                    NumberOfSupports = supportNodes.Count(),
                    Center = center,
                    Diameter = diameter.ToString("N3")
                },
                SupportLoadsCollection = new HashSet<SupportLoads>()
            };

            for (int iSupport = 0; iSupport < supportNodes.Count(); iSupport++)
            {
                SupportLoads supportLoads = RetrieveSupportLoadsFor(supportNodes[iSupport], loadCases, output);
                _ = pcdLoads.SupportLoadsCollection.Add(supportLoads);
            }

            pcdLoads.SupportLoadsSummary = SupportLoadsSummaryService.Generate(pcdLoads);

            return pcdLoads;
        }

        private static SupportLoads RetrieveSupportLoadsFor(Node support, List<LoadCase> loadCases, OSOutputUI output)
        {
            SupportLoads supportLoads = new SupportLoads() { Support = support, Loads = new HashSet<LoadCaseForces>() };

            loadCases.ForEach(lc => supportLoads.Loads.Add(StaadOutputServices.RetrieveLoadCaseForces(output, support.Id, lc.Id)));

            return supportLoads;
        }

        #endregion

        #region Private Helpers - Staad Operations

        private static HashSet<EntityGroup> GetSupportEntityGroups(OSGeometryUI geometry)
        {
            HashSet<EntityGroup<Node>> supportEntityGroups = StaadGeometryServices.GetEntityGroupsOfType<Node>(geometry).Where(g => g.GroupName.Contains("_SUP_")).ToHashSet();

            return EntityGroup<Node>.Transform(supportEntityGroups);
        }

        private static List<LoadCase> GetPrimaryLoadCasesForLoadDataGeneration(OSLoadUI load, IEnumerable<int> reportLoadCasesIds)
        {
            List<LoadCase> loadCases = new List<LoadCase>();

            foreach (int lcId in reportLoadCasesIds)
            {
                string lcTitle = (string)load.GetLoadCaseTitle(lcId);
                loadCases.Add(new LoadCase(lcId) { Title = lcTitle });
            }

            return loadCases;
        }

        #endregion
    }
}

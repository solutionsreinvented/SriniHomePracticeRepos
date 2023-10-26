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

namespace ReInvented.Domain.Reporting.Services
{
    public class FoundationLoadDataService
    {
        #region File Operations

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
            string seializedContent = "const FoundationLoadData = " + serializer.Serialize(foundationLoadData, JsonSerializerSettingsProvider.MinifiedSettings());

            File.WriteAllText(Path.Combine(projectDataDirectory.FullName, ReportFileNames.ContentsFoundationLoadData), seializedContent);
        }

        #endregion

        #region Public Functions

        public FoundationLoadData GenerateReportContent(IProjectInfo projectInfo, IEnumerable<int> reportLoadsIds)
        {
            StaadModel model = new StaadModel();
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<Node> nodes = RetrieveAllNodesFromStaadModel(openStaad.Geometry);
            HashSet<EntityGroup> pcdSupportGroups = GetSupportEntityGroups(openStaad.Geometry);
            List<LoadCase> loadCases = GetPrimaryLoadCasesForLoadDataGeneration(openStaad.Load, reportLoadsIds);

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, nodes, loadCases, projectInfo, model.ModelName);

            return foundationLoadData;
        }

        public FoundationLoadData GenerateReportContent(StaadModel model, IProjectInfo projectInfo, IEnumerable<int> reportLoadsIds)
        {
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<EntityGroup> pcdSupportGroups = model.EntityGroups.Where(g => g.GroupName.Contains("_SUP_") && g.EntityType == EntityType.Nodes).ToHashSet();
            List<LoadCase> loadCases = model.PrimaryLoads.Where(pl => reportLoadsIds.Contains(pl.Id)).OrderBy(lc => lc.Id).ToList();

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, model.Nodes, loadCases, projectInfo, model.FileName);

            return foundationLoadData;
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

            string generatedOn = DateTime.Now.ToString("F");

            FoundationLoadData foundationLoadData = new FoundationLoadData()
            {
                GeneratedOn = generatedOn,
                DataSourceInformation = new DataSourceInformation()
                {
                    ///TODO: Hard coded properties below shall be replaced with user input.
                    Engineer = projectInfo.Originator.FullName,
                    PreparedOn = generatedOn,
                    ProjectCode = projectInfo.Code,
                    ProjectName = projectInfo.Name,
                    StaadFilename = staadFilename
                }
            };

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
            int roundDigits = 1;

            SupportLoads supportLoads = new SupportLoads() { Support = support, Loads = new HashSet<LoadCaseForces>() };

            for (int iLoadCase = 0; iLoadCase < loadCases.Count(); iLoadCase++)
            {
                object reactions = new double[6];

                output.GetSupportReactions(supportLoads.Support.Id, loadCases[iLoadCase].Id, ref reactions);

                _ = supportLoads.Loads.Add(new LoadCaseForces()
                {
                    Id = loadCases[iLoadCase].Id,
                    Fx = Math.Round(((double[])reactions)[0], roundDigits),
                    Fy = Math.Round(((double[])reactions)[1], roundDigits),
                    Fz = Math.Round(((double[])reactions)[2], roundDigits),
                    Mx = Math.Round(((double[])reactions)[3], roundDigits),
                    My = Math.Round(((double[])reactions)[4], roundDigits),
                    Mz = Math.Round(((double[])reactions)[5], roundDigits)
                });
            }

            return supportLoads;
        }

        #endregion

        #region Private Helpers - Staad Operations

        private static HashSet<Node> RetrieveAllNodesFromStaadModel(OSGeometryUI geometry)
        {
            int nodesCount = (int)geometry.GetNodeCount();

            HashSet<Node> nodes = new HashSet<Node>(nodesCount);

            object nodesIds = new int[nodesCount];
            geometry.GetNodeList(ref nodesIds);

            foreach (int iNodeId in (int[])nodesIds)
            {
                object xCoordinate = 0.0;
                object yCoordinate = 0.0;
                object zCoordinate = 0.0;

                geometry.GetNodeCoordinates(iNodeId, ref xCoordinate, ref yCoordinate, ref zCoordinate);
                _ = nodes.Add(new Node() { Id = iNodeId, X = (double)xCoordinate, Y = (double)yCoordinate, Z = (double)zCoordinate });
            }

            return nodes;
        }

        private static HashSet<EntityGroup> GetSupportEntityGroups(OSGeometryUI geometry)
        {
            int nodeTypeGroupCount = (int)geometry.GetGroupCount(GroupType.Nodes);

            object groupNames = new string[nodeTypeGroupCount];

            geometry.GetGroupNames(GroupType.Nodes, ref groupNames);

            HashSet<EntityGroup> entityGroups = new HashSet<EntityGroup>();

            foreach (string groupName in (string[])groupNames)
            {
                if (groupName.Contains("_SUP_"))
                {
                    int entityCount = (int)geometry.GetGroupEntityCount(groupName);
                    object entities = new int[entityCount];
                    geometry.GetGroupEntities(groupName, ref entities);

                    EntityGroup entityGroup = new EntityGroup()
                    {
                        EntityType = EntityType.Nodes,
                        GroupName = groupName,
                        Entities = new HashSet<int>()
                    };

                    ((int[])entities).ToList().ForEach(e => entityGroup.Entities.Add(e));

                    _ = entityGroups.Add(entityGroup);
                }
            }

            return entityGroups;
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

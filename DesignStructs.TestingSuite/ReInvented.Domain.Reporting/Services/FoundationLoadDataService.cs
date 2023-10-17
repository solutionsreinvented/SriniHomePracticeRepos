using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Geometry.Models;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public class FoundationLoadDataService
    {
        public FoundationLoadData GenerateReportContent(IProjectInfo projectInfo)
        {
            StaadModel model = new StaadModel();
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<Node> nodes = RetrieveAllNodesFromStaadModel(openStaad.Geometry);
            HashSet<EntityGroup> pcdSupportGroups = GetSupportEntityGroups(openStaad.Geometry);
            List<LoadCase> loadCases = GetPrimaryLoadCasesForLoadDataGeneration(openStaad.Load);

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, nodes, loadCases, projectInfo, model.ModelName);

            return foundationLoadData;
        }

        public FoundationLoadData GenerateReportContent(StaadModel model, IProjectInfo projectInfo)
        {
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<EntityGroup> pcdSupportGroups = model.EntityGroups.Where(g => g.GroupName.Contains("_SUP_") && g.EntityType == EntityType.Nodes).ToHashSet();
            List<LoadCase> loadCases = model.PrimaryLoads.Where(pl => pl.Id >= 601 && pl.Id <= 615).ToList();

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, model.Nodes, loadCases, projectInfo, model.FileName);

            return foundationLoadData;
        }


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
            double diameter = Node.Diameter(supportNodes.First(), center);

            PCDLoads pcdLoads = new PCDLoads()
            {
                PCD = groupName.Replace("_SUP_", ""),
                SupportsInformation = new SupportsInformation()
                {
                    NumberOfSupports = supportNodes.Count(),
                    Center = center,
                    Diameter = diameter
                },
                SupportLoadsCollection = new HashSet<SupportLoads>()
            };

            for (int iSupport = 0; iSupport < supportNodes.Count(); iSupport++)
            {
                SupportLoads supportLoads = RetrieveSupportLoadsFor(supportNodes[iSupport].Id, loadCases, output);
                _ = pcdLoads.SupportLoadsCollection.Add(supportLoads);
            }

            pcdLoads.SupportLoadsSummary = SupportLoadsSummaryService.Generate(pcdLoads);

            return pcdLoads;
        }

        private static SupportLoads RetrieveSupportLoadsFor(int supportId, List<LoadCase> loadCases, OSOutputUI output)
        {
            SupportLoads supportLoads = new SupportLoads() { NodeNumber = supportId, Loads = new HashSet<LoadCaseForces>() };

            for (int iLoadCase = 0; iLoadCase < loadCases.Count(); iLoadCase++)
            {
                object reactions = new double[6];

                output.GetSupportReactions(supportLoads.NodeNumber, loadCases[iLoadCase].Id, ref reactions);

                _ = supportLoads.Loads.Add(new LoadCaseForces()
                {
                    Id = loadCases[iLoadCase].Id,
                    Fx = ((double[])reactions)[0],
                    Fy = ((double[])reactions)[1],
                    Fz = ((double[])reactions)[2],
                    Mx = ((double[])reactions)[3],
                    My = ((double[])reactions)[4],
                    Mz = ((double[])reactions)[5],
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

        private static List<LoadCase> GetPrimaryLoadCasesForLoadDataGeneration(OSLoadUI load)
        {
            List<LoadCase> loadCases = new List<LoadCase>();

            IEnumerable<int> primaryLoadCaseNumbers = Enumerable.Range(601, 15);

            foreach (int lcNumber in primaryLoadCaseNumbers)
            {
                string lcTitle = (string)load.GetLoadCaseTitle(lcNumber);
                loadCases.Add(new LoadCase(lcNumber) { Title = lcTitle });
            }

            return loadCases;
        }

        #endregion

    }
}

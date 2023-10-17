using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Geometry.Models;
using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public class FoundationLoadDataService
    {
        public FoundationLoadData GenerateReportContent()
        {
            /// TODO: Implementation pending
            ///       This function will be used when the StaadModel is already created and the analysis results are available.
            return null;
        }

        public FoundationLoadData GenerateReportContent(StaadModel model, ProjectInfo projectInfo)
        {

            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<EntityGroup> pcdSupportGroups = model.EntityGroups.Where(g => g.GroupName.Contains("_SUP_") && g.EntityType == EntityType.Nodes).ToHashSet();
            List<LoadCase> loadCases = model.PrimaryLoads.Where(pl => pl.Id >= 601 && pl.Id <= 615).ToList();

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
                    StaadFilename = model.FileName
                }
            };

            foundationLoadData.LoadCases = new Dictionary<int, string>();
            loadCases.ForEach(lc => foundationLoadData.LoadCases.Add(lc.Id, lc.Title));

            foundationLoadData.PCDLoadsCollection = RetrievePCDLoadsCollection(pcdSupportGroups, model.Nodes, loadCases, output);

            return foundationLoadData;

        }


        #region Private Helpers

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
            SupportLoads supportLoads = new SupportLoads() { NodeNumber = supportId };

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

    }
}

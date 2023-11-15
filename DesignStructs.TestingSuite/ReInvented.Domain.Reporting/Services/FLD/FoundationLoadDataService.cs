﻿using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Geometry.Models;
using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Reporting.Services
{
    public static class OSOutputExtensionsLocal
    {
        public static HashSet<StaticCheckResult> GetStaticCheckResults(OSOutputUI output, HashSet<int> loadCaseIds)
        {
            HashSet<StaticCheckResult> staticCheckResults = new HashSet<StaticCheckResult>();

            foreach (int lcId in loadCaseIds)
            {
                object forces = new double[6];
                object reactions = new double[6];
                
                bool result = output.GetStaticCheckResult(lcId, forces, reactions);

                StaticCheckResult staticCheckResult = new StaticCheckResult(lcId) { Forces = Forces.ParseFromArray((double[])forces), Reactions = Forces.ParseFromArray((double[])reactions) };

                _ = staticCheckResults.Add(staticCheckResult);
            }

            return staticCheckResults;
        }

    }


    public class FoundationLoadDataService
    {
        #region Public Functions

        public static FoundationLoadData Generate(IProjectData projectData, IEnumerable<int> reportLoadsIds)
        {
            StaadModel model = new StaadModel();
            OpenSTAAD instance = model.StaadWrapper.StaadInstance;
            OSOutputUI output = instance.Output;
            OSGeometryUI geometry = instance.Geometry;
            HashSet<StaticCheckResult> staticsCheck = OSOutputExtensionsLocal.GetStaticCheckResults(output, reportLoadsIds.ToHashSet());

            HashSet<Node> nodes = geometry.GetAllNodes();
            HashSet<EntityGroup> pcdSupportGroups = GetSupportEntityGroups(instance.Geometry);
            List<LoadCase> loadCases = GetPrimaryLoadCasesForLoadDataGeneration(instance.Load, reportLoadsIds);

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, nodes, loadCases, projectData, model.ModelName);

            return foundationLoadData;
        }

        public static FoundationLoadData Generate(StaadModel model, IProjectData projectData, IEnumerable<int> reportLoadsIds)
        {
            OpenSTAAD openStaad = model.StaadWrapper.StaadInstance;
            OSOutputUI output = openStaad.Output;

            HashSet<EntityGroup> pcdSupportGroups = model.EntityGroups.Where(g => g.GroupName.Contains("_SUP_") && g.EntityType == EntityType.Nodes).ToHashSet();
            List<LoadCase> loadCases = model.PrimaryLoads.Where(pl => reportLoadsIds.Contains(pl.Id)).OrderBy(lc => lc.Id).ToList();

            FoundationLoadData foundationLoadData = GenerateFoundationLoadData(output, pcdSupportGroups, model.Nodes, loadCases, projectData, model.FileName);

            return foundationLoadData;
        }

        #endregion

        #region Private Helpers

        private static FoundationLoadData GenerateFoundationLoadData(OSOutputUI output, HashSet<EntityGroup> pcdSupportGroups, HashSet<Node> nodes,
                                                                     List<LoadCase> loadCases, IProjectData projectData, string staadFilename)
        {
            if (!output.AreResultsAvailable())
            {
                return null;
            }

            FoundationLoadData foundationLoadData = new FoundationLoadData
            {
                LoadCases = new Dictionary<int, string>()
            };

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

            loadCases.ForEach(lc => supportLoads.Loads.Add(output.RetrieveLoadCaseForces(support.Id, lc.Id)));
            
            return supportLoads;
        }

        #endregion

        #region Private Helpers - Staad Operations

        private static HashSet<EntityGroup> GetSupportEntityGroups(OSGeometryUI geometry)
        {
            HashSet<EntityGroup<Node>> supportEntityGroups = geometry.GetEntityGroupsOfType<Node>().Where(g => g.GroupName.Contains("_SUP_")).ToHashSet();

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

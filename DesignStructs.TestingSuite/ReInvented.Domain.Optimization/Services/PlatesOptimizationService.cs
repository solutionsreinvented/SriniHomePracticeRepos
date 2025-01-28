using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Optimization.Models;
using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.Domain.Optimization.Services
{
    public class PlatesOptimizationService
    {
        #region Parameterized Constructor

        public PlatesOptimizationService(OpenStaadWrapper wrapper, PlateOptimizationCriteria optimizationCriteria)
        {
            Wrapper = wrapper;
            Geometry = wrapper.Geometry;
            Property = wrapper.Property;
            Load = wrapper.Load;
            Output = wrapper.Output;
            Criteria = optimizationCriteria;
        }

        #endregion

        #region Readonly Properties

        public HashSet<double> CommonThicknesses => new HashSet<double>() { 5, 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 20, 22, 25, 28, 30, 32, 35, 38, 40, 45, 50, 55, 60, 65, 70, 75, 80, 90, 100, 110, 120, 125, 130, 140, 150, 160, 170, 180, 190, 200 };

        public OpenStaadWrapper Wrapper { get; private set; }

        public OSGeometryUI Geometry { get; private set; }

        public OSPropertyUI Property { get; private set; }

        public OSLoadUI Load { get; private set; }

        public OSOutputUI Output { get; private set; }

        public PlateOptimizationCriteria Criteria { get; set; }

        public HashSet<ILoadCase> LoadCases { get; private set; }

        #endregion

        #region Public Functions

        public PlatesOptimizationReport GenerateReport(IEnumerable<ILoadCase> loadCases)
        {
            return new PlatesOptimizationReport { Criteria = Criteria, Results = OptimizeAll(loadCases) };
        }

        public HashSet<PlateGroupDesignResult> OptimizeAll(IEnumerable<ILoadCase> loadCases)
        {
            IEnumerable<Plate> allPlates = Geometry.GetAllEntities<Plate>(Criteria.ThreadCount);
            HashSet<LoadCase> plc = Load.GetAllPrimaryLoadCases();

            IEnumerable<string> groupNames = Geometry.GetEntityGroups<Plate>(Criteria.ThreadCount)
                                                     .Where(eg => eg.Entities.Count() > 0)
                                                     .OrderByDescending(eg => Plate.MaxYCoordinate(eg.Entities.OrderByDescending(e => Plate.MaxYCoordinate(e)).First()))
                                                     .Where(eg => !InExclusionList(eg.GroupName))
                                                     .Select(eg => eg.GroupName).ToHashSet();

            HashSet<PlateGroupDesignResult> designResults = OptimizeGroups(groupNames, loadCases);

            return designResults;
        }

        public HashSet<PlateGroupDesignResult> OptimizeGroups(IEnumerable<string> groupNames, IEnumerable<ILoadCase> loadCases)
        {
            ConcurrentBag<PlateGroupDesignResult> groupDesignResults = new ConcurrentBag<PlateGroupDesignResult>();

            groupNames.AsParallel().WithDegreeOfParallelism(Criteria.ThreadCount).ForAll(gn =>
            {
                PlateGroupDesignResult designResult = OptimizeGroup(gn, loadCases);
                groupDesignResults.Add(designResult);
            });

            return groupDesignResults.ToHashSet();
        }

        public PlateGroupDesignResult OptimizeGroup(string groupName, IEnumerable<ILoadCase> loadCases)
        {
            IEnumerable<Plate> plates = Geometry.GetEntitiesInGroup<Plate>(groupName, 1);
            double currentThickness = Property.GetPlateThickness(plates.First().Id).A * 1000;

            IEnumerable<PlateCenterResults> results = Wrapper.GetPlateCenterResultsForGroup(groupName, loadCases, Criteria.ThreadCount);
            PlateGroupStressSummary governingResult = results.GetGoverningPlateGroupStressSummary(Criteria.LimitingStress);

            PlateGroupDesignResult designResult = new PlateGroupDesignResult(groupName, governingResult, currentThickness);

            double maxAbsVonMises = governingResult.GoverningResults.VonMises.AbsoluteMaximum / 1000;

            if (maxAbsVonMises <= Criteria.LimitingStress && governingResult.PercentPlatesExceeding > Criteria.AllowedPercentPlatesExceedance)
            {
                designResult.DesignThickness *= maxAbsVonMises / Criteria.LimitingStress;
            }

            designResult.DesignThickness = Math.Max(designResult.DesignThickness + Criteria.CorrosionAllowance, Criteria.MinimumThickness);
            designResult.DesignThickness = CommonThicknesses.Where(t => t >= designResult.DesignThickness).OrderBy(t => t).FirstOrDefault();

            designResult.CorrosionAllowance = Criteria.CorrosionAllowance;
            designResult.MinimumThickness = Criteria.MinimumThickness;

            return designResult;
        }


        #endregion

        #region Private Helpers

        private bool InExclusionList(string groupName)
        {
            return Criteria.ExcludedGroupNames.Any(eg => groupName.Contains(eg));
            /* groupName.Contains("TANK") || groupName.Contains("COMP") || groupName.Contains("CENTRE") || groupName.Contains("LAUNDER"); */
        }

        #endregion
    }
}

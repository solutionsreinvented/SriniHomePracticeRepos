using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using OpenSTAADUI;

using ReInvented.DataAccess;
using ReInvented.DataAccess.Services;
using ReInvented.Domain.Optimization.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace DevDrive.Services
{
    public class PlatesOptimizationService
    {
        #region Parameterized Constructor

        public PlatesOptimizationService(OpenStaadWrapper wrapper, PlateOptimizationCriteria optimizationCriteria)
        {
            Wrapper = wrapper;
            Geometry = wrapper.Geometry as OSGeometryUI;
            Property = wrapper.Property as OSPropertyUI;
            Criteria = optimizationCriteria;
            OutputFiles = new OutputFiles(wrapper.StaadInstance.GetStaadFileFullPath());
        }

        #endregion

        #region Readonly Properties

        public HashSet<double> CommonThicknesses => new HashSet<double>() { 5, 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 20, 22, 25, 28, 30, 32, 35, 38, 40, 45, 50, 55, 60, 65, 70, 75, 80, 90, 100, 110, 120, 125, 130, 140, 150, 160, 170, 180, 190, 200 };
        public OpenStaadWrapper Wrapper { get; private set; }
        public OSGeometryUI Geometry { get; private set; }
        public OSPropertyUI Property { get; private set; }
        public PlateOptimizationCriteria Criteria { get; set; }
        public OutputFiles OutputFiles { get; private set; }

        #endregion

        #region Public Functions

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, IEnumerable<LoadCase> loadCases)
        {
            IEnumerable<Plate> plates = Geometry.GetEntitiesInGroup<Plate>(groupName, 1);
            double currentThickness = Property.GetPlateThickness(plates.First().Id).A * 1000;

            IEnumerable<PlateCenterResults> results = Wrapper.GetPlateCenterResultsForGroup(groupName, loadCases, Criteria.ThreadCount);
            PlateGroupStressSummary governingResult = results.GetGoverningPlateGroupStressSummary(Criteria.LimitingStress);

            PlateGroupDesignResult designResult = new PlateGroupDesignResult(groupName, governingResult, currentThickness);

            double maxAbsVonMises = governingResult.GoverningResults.VonMises.AbsoluteMaximum / 1000;

            if (maxAbsVonMises <= Criteria.LimitingStress || governingResult.PercentPlatesExceeding > Criteria.AllowedPercentPlatesExceedance)
            {
                designResult.DesignThickness *= maxAbsVonMises / Criteria.LimitingStress;
            }

            designResult.DesignThickness = Math.Max(designResult.DesignThickness + Criteria.CorrosionAllowance, Criteria.MinimumThickness);
            designResult.DesignThickness = CommonThicknesses.Where(t => t >= designResult.DesignThickness).OrderBy(t => t).FirstOrDefault();

            designResult.CorrosionAllowance = Criteria.CorrosionAllowance;
            designResult.MinimumThickness = Criteria.MinimumThickness;

            return designResult;
        }

        public List<PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, IEnumerable<LoadCase> loadCases)
        {
            ConcurrentBag<PlateGroupDesignResult> groupDesignResults = new ConcurrentBag<PlateGroupDesignResult>();

            groupNames.AsParallel().WithDegreeOfParallelism(Criteria.ThreadCount).ForAll(gn =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                PlateGroupDesignResult designResult = OptimizePlateGroup(gn, loadCases); ///OptimizePlateGroup(gn, Criteria.MinimumThickness, Criteria.CorrosionAllowance, loadCases, Criteria.AllowedPercentPlatesExceedance);

                stopwatch.Stop();
                Console.WriteLine($"Plate group {gn} optimization is processed in {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)}");

                groupDesignResults.Add(designResult);
            });

            return groupDesignResults.ToList();
        }

        #endregion

        public void WriteResultsToFile(List<PlateGroupDesignResult> designResults)
        {
            JsonDataSerializer<List<PlateGroupDesignResult>> serializer = new JsonDataSerializer<List<PlateGroupDesignResult>>();
            string serialized = "const content = " + serializer.Serialize(designResults, JsonSerializerSettingsProvider.Minified);

            File.WriteAllText(OutputFiles.JsonOutputFileFullPath, serialized);
        }

    }
}

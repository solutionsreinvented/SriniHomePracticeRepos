using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Sections.Domain.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Extensions;
using ReInvented.StaadPro.Interactivity.Models;

namespace DevDrive.Services
{

    public sealed class PlateGroupDesignResult
    {
        public PlateGroupDesignResult()
        {

        }

        public PlateGroupDesignResult(string groupName, PlateGroupStressSummary stressSummary, double currentThickness, double designThickness)
        {
            GroupName = groupName;
            StressSummary = stressSummary;
            CurrentThickness = currentThickness;
            DesignThickness = designThickness;
        }

        public string GroupName { get; set; }

        public PlateGroupStressSummary StressSummary { get; set; }

        public double DesignThickness { get; set; }
        public double CurrentThickness { get; set; }
        public double CorrosionAllowance { get; set; }
        public double MinimumThickness { get; set; }
    }


    public class PlatesOptimizationService
    {
        #region Parameterized Constructor

        public PlatesOptimizationService(OpenStaadWrapper wrapper, MaterialGrade materialGrade, int threadCount)
        {
            Wrapper = wrapper;
            Geometry = wrapper.Geometry as OSGeometryUI;
            Property = wrapper.Property as OSPropertyUI;
            MaterialGrade = materialGrade;
            PlateLimitingStress = materialGrade.Fy;
            ThreadCount = threadCount;
        }

        public PlatesOptimizationService(OpenStaadWrapper wrapper, MaterialGrade materialGrade) : this(wrapper, materialGrade, 1)
        {

        }

        #endregion

        #region Readonly Properties

        public HashSet<double> CommonThicknesses => new HashSet<double>() { 5, 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 20, 22, 25, 28, 30, 32, 35, 38, 40, 45, 50, 55, 60, 65, 70, 75, 80, 90, 100, 110, 120, 125, 130, 140, 150, 160, 170, 180, 190, 200 };

        public double PlateLimitingStress { get; private set; }

        public OpenStaadWrapper Wrapper { get; private set; }

        public OSGeometryUI Geometry { get; private set; }

        public OSPropertyUI Property { get; private set; }

        public int ThreadCount { get; private set; }

        public MaterialGrade MaterialGrade { get; private set; }

        #endregion

        #region Public Functions

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            IEnumerable<Plate> plates = Geometry.GetEntitiesInGroup<Plate>(groupName, 1);
            double currentThickness = Property.GetPlateThickness(plates.First().Id).A * 1000;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IEnumerable<PlateCenterResults> results = Wrapper.GetPlateCenterResultsForGroup(groupName, loadCases, ThreadCount);
            stopwatch.Stop();
            var elapsed = TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds);
            PlateGroupStressSummary governingResult = results.GetGoverningPlateGroupStressSummary(PlateLimitingStress);

            PlateGroupDesignResult designResult = new PlateGroupDesignResult(groupName, governingResult, currentThickness, currentThickness);

            if (governingResult.PercentPlatesExceeding <= allowedPercentPlatesToExceed)
            {
                designResult.DesignThickness *= governingResult.GoverningResults.VonMises.AbsoluteMaximum / 1000 / PlateLimitingStress;
                designResult.DesignThickness = CommonThicknesses.Where(t => t >= designResult.DesignThickness).OrderBy(t => t).FirstOrDefault();
            }

            return designResult;
        }

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, double minimumThickness, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            return OptimizePlateGroup(groupName, minimumThickness, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, double minimumThickness, double corrosionAllowance, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            PlateGroupDesignResult result = OptimizePlateGroup(groupName, loadCases, allowedPercentPlatesToExceed);
            double designThickness = Math.Max(result.DesignThickness + corrosionAllowance, minimumThickness);
            result.DesignThickness = CommonThicknesses.Where(t => t >= designThickness).OrderBy(t => t).FirstOrDefault();

            result.CorrosionAllowance = corrosionAllowance;
            result.MinimumThickness = minimumThickness;

            return result;
        }

        public List<PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            return OptimizePlateGroups(groupNames, 0.0, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public List<PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, double minimumThickness, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            return OptimizePlateGroups(groupNames, minimumThickness, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public List<PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, double minimumThickness, double corrosionAllowance,
                                                                      IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            ConcurrentBag<PlateGroupDesignResult> groupDesignResults = new ConcurrentBag<PlateGroupDesignResult>();

            groupNames.AsParallel().WithDegreeOfParallelism(ThreadCount).ForAll(gn =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                PlateGroupDesignResult designResult = OptimizePlateGroup(gn, minimumThickness, corrosionAllowance, loadCases, allowedPercentPlatesToExceed);

                stopwatch.Stop();
                Console.WriteLine($"Plate group {gn} optimization is processed in {TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds)}");

                groupDesignResults.Add(designResult);
            });

            return groupDesignResults.ToList();
        }


        #endregion
    }
}

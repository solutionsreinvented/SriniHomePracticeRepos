using System;
using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

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

        public PlateGroupDesignResult(PlateGroupStressSummary stressSummary, double designThickness)
        {
            StressSummary = stressSummary;
            Thickness = designThickness;
        }

        public PlateGroupStressSummary StressSummary { get; set; }

        public double Thickness { get; set; }
    }


    public class PlatesOptimizationService
    {
        #region Parameterized Constructor

        public PlatesOptimizationService(OpenStaadWrapper wrapper, double limitingPlateStress)
        {
            Wrapper = wrapper;
            Geometry = wrapper.Geometry as OSGeometryUI;
            Property = wrapper.Property as OSPropertyUI;
            PlateLimitingStress = limitingPlateStress;
        }

        #endregion

        #region Readonly Properties

        public HashSet<double> CommonThicknesses => new HashSet<double>() { 5, 6, 8, 9, 10, 12, 13, 14, 15, 16, 17, 18, 20, 22, 25, 28, 30, 32, 35, 38, 40, 45, 50, 55, 60, 65, 70, 75, 80, 90, 100, 110, 120, 125, 130, 140, 150, 160, 170, 180, 190, 200 };

        public double PlateLimitingStress { get; private set; }

        public OpenStaadWrapper Wrapper { get; private set; }

        public OSGeometryUI Geometry { get; private set; }

        public OSPropertyUI Property { get; private set; }

        #endregion

        #region Public Functions

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            IEnumerable<Plate> plates = Geometry.GetPlatesFromPlateGroup(groupName);
            double currentThickness = Property.GetPlateThickness(plates.First().Id).A * 1000;

            IEnumerable<PlateCenterResults> results = Wrapper.GetPlateCenterResultsForGroup(groupName, loadCases);
            PlateGroupStressSummary governingResult = results.GetGoverningPlateGroupStressSummary(PlateLimitingStress);

            PlateGroupDesignResult designResult = new PlateGroupDesignResult(governingResult, currentThickness);

            if (governingResult.PercentPlatesExceeding <= allowedPercentPlatesToExceed)
            {
                designResult.Thickness *= governingResult.GoverningResults.VonMises.AbsoluteMaximum / 1000 / PlateLimitingStress;
                designResult.Thickness = CommonThicknesses.Where(t => t >= designResult.Thickness).OrderBy(t => t).FirstOrDefault();
            }

            return designResult;
        }

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, double minimumThickness, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            //PlateGroupDesignResult result = OptimizePlateGroup(groupName, loadCases, allowedPercentPlatesToExceed);
            //double designThickness = Math.Max(result.Thickness, minimumThickness);
            //result.Thickness = CommonThicknesses.Where(t => t >= designThickness).OrderBy(t => t).FirstOrDefault();

            //return result;
            return OptimizePlateGroup(groupName, minimumThickness, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public PlateGroupDesignResult OptimizePlateGroup(string groupName, double minimumThickness, double corrosionAllowance, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            PlateGroupDesignResult result = OptimizePlateGroup(groupName, loadCases, allowedPercentPlatesToExceed);
            double designThickness = Math.Max(result.Thickness + corrosionAllowance, minimumThickness);
            result.Thickness = CommonThicknesses.Where(t => t >= designThickness).OrderBy(t => t).FirstOrDefault();

            return result;
        }

        public Dictionary<string, PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            //Dictionary<string, PlateGroupDesignResult> groupDesignResults = new Dictionary<string, PlateGroupDesignResult>();

            //foreach (string groupName in groupNames.ToHashSet())
            //{
            //    PlateGroupDesignResult designResult = OptimizePlateGroup(groupName, loadCases, allowedPercentPlatesToExceed);
            //    groupDesignResults.Add(groupName, designResult);
            //}

            //return groupDesignResults;
            return OptimizePlateGroups(groupNames, 0.0, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public Dictionary<string, PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, double minimumThickness, IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            //Dictionary<string, PlateGroupDesignResult> groupDesignResults = new Dictionary<string, PlateGroupDesignResult>();

            //foreach (string groupName in groupNames.ToHashSet())
            //{
            //    PlateGroupDesignResult designResult = OptimizePlateGroup(groupName, minimumThickness, loadCases, allowedPercentPlatesToExceed);
            //    groupDesignResults.Add(groupName, designResult);
            //}

            //return groupDesignResults;
            return OptimizePlateGroups(groupNames, minimumThickness, 0.0, loadCases, allowedPercentPlatesToExceed);
        }

        public Dictionary<string, PlateGroupDesignResult> OptimizePlateGroups(IEnumerable<string> groupNames, double minimumThickness, double corrosionAllowance,
                                                                              IEnumerable<LoadCase> loadCases, double allowedPercentPlatesToExceed = 15.0)
        {
            Dictionary<string, PlateGroupDesignResult> groupDesignResults = new Dictionary<string, PlateGroupDesignResult>();

            foreach (string groupName in groupNames.ToHashSet())
            {
                PlateGroupDesignResult designResult = OptimizePlateGroup(groupName, minimumThickness, corrosionAllowance, loadCases, allowedPercentPlatesToExceed);
                groupDesignResults.Add(groupName, designResult);
            }

            return groupDesignResults;
        }

        #endregion
    }
}

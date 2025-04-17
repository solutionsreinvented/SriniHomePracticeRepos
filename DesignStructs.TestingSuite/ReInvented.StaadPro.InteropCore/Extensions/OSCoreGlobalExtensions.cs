using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreGlobalExtensions
    {
        public static StaadModel GetStaadModel(string staadFileFullPath)
        {
            return OSGlobalExtensions.GetStaadModel(staadFileFullPath);
        }

        public static IEnumerable<PlateCenterResults> GetPlateCenterResultsForGroup(this OpenStaadCoreWrapper wrapper, string plateGroupName, IEnumerable<ILoadCase> loadCases)
        {
            return wrapper.OpenStaadWrapper.GetPlateCenterResultsForGroup(plateGroupName, loadCases);
        }

        public static PlateGroupStressSummary GetGoverningPlateGroupStressSummary(this IEnumerable<PlateCenterResults> results, double limitingStress)
        {
            return OSGlobalExtensions.GetGoverningPlateGroupStressSummary(results, limitingStress);
        }
    }
}

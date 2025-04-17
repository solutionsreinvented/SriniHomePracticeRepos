using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreGlobalExtensionsParallel
    {
        public static IEnumerable<PlateCenterResults> GetPlateCenterResultsAllPlates(this OpenStaadCoreWrapper coreWrapper, IEnumerable<ILoadCase> loadCases, int nThreads)
        {
            return coreWrapper.OpenStaadWrapper.GetPlateCenterResultsAllPlates(loadCases, nThreads);
        }

        public static IEnumerable<PlateCenterResults> GetPlateCenterResultsForGroup(this OpenStaadCoreWrapper coreWrapper, string plateGroupName, IEnumerable<ILoadCase> loadCases, int nThreads)
        {
            return coreWrapper.OpenStaadWrapper.GetPlateCenterResultsForGroup(plateGroupName, loadCases);
        }
        
        public static PlateGroupStressSummary GetGoverningPlateGroupStressSummary(this IEnumerable<PlateCenterResults> results, double limitingStress, int nThreads)
        {
            return OSGlobalExtensionsParallel.GetGoverningPlateGroupStressSummary(results, limitingStress, nThreads);
        }
    }
}

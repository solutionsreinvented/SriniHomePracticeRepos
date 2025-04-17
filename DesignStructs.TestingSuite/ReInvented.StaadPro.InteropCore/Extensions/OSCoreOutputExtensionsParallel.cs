using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreOutputExtensionsParallel
    {
        //[Obsolete("Thorough testing is not done. Do not use until released for use", true)]
        public static IEnumerable<PlateCenterResults> GetPlateCenterResults(this OSCoreOutput output, IEnumerable<ILoadCase> loadCases, IEnumerable<Plate> plates, int nThreads)
        {
            return output.ComObject.GetPlateCenterResults(loadCases, plates, nThreads);
        }

        public static HashSet<StaticCheckResult> GetStaticCheckResults(this OSCoreOutput output, IEnumerable<int> loadCaseIds, int nThreads)
        {
            return output.ComObject.GetStaticCheckResults(loadCaseIds, nThreads);
        }
    }
}

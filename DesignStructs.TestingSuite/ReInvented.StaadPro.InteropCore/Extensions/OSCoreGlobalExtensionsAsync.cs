using System.Collections.Generic;
using System.Threading.Tasks;

using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreGlobalExtensionsAsync
    {
        public static async Task<IEnumerable<PlateCenterResults>> GetPlateCenterResultsForAllPlatesAsync(this OpenStaadCoreWrapper wrapper, IEnumerable<ILoadCase> loadCases, int nThreads)
        {
            return await wrapper.OpenStaadWrapper.GetPlateCenterResultsForAllPlatesAsync(loadCases, nThreads);
        }
    }
}

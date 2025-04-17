using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreLoadExtensionsParallel
    {
        public static OSCoreLoad AddSelfweightInXYZTo(this OSCoreLoad load, int loadCaseId, IEnumerable<int> entities, int nThreads, SelftWeightDirection loadDirection = SelftWeightDirection.GlobalY, double factor = -1.0)
        {
            load.AddSelfweightInXYZTo(loadCaseId, entities, nThreads, loadDirection, factor);
            return load;
        }
    }
}

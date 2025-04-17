using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreSupportExtensionsParallel
    {
        public static HashSet<Node> GetAllSupportNodes(this OSCoreSupport support, HashSet<Node> allNodes, int nThreads)
        {
            return support.ComObject.GetAllSupportNodes(allNodes, nThreads);
        }

        public static HashSet<Node> GetAllSupportNodes(this OSCoreSupport support, OSCoreGeometry geometry, int nThreads)
        {
            return support.ComObject.GetAllSupportNodes(geometry.ComObject, nThreads);
        }
    }
}

using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSCoreSupportExtensions
    {
        public static int GetSupportNodesCount(this OSCoreSupport support)
        {
            return support.ComObject.GetSupportCount();
        }

        public static IEnumerable<int> GetSupportNodesIds(this OSCoreSupport support)
        {
            return support.ComObject.GetSupportNodesIds();
        }

        public static Releases GetSupportInformation(this OSCoreSupport support, int nodeNumber)
        {
            return support.ComObject.GetSupportInformation(nodeNumber);
        }
    }
}

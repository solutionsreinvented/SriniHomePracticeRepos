using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OpenStaadCoreExtensions
    {
        public static HashSet<MemberForces> RetrieveMemberForces(this OpenStaadCore openStaad, IEnumerable<int> beams, IEnumerable<int> loadCases)
        {
            return openStaad.ComObject.RetrieveMemberForces(beams, loadCases);
        }
    }
}

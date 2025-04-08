using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.StaadPro.InteropCore.Interfaces
{
    public interface IOSRootCore
    {
        HashSet<MemberForces> RetrieveMemberForces(IEnumerable<int> beams, IEnumerable<int> loadCases);
    }
}

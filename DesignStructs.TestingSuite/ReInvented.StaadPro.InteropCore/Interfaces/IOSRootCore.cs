using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;

namespace ReInvented.StaadPro.InteropCore.Interfaces
{
    public interface IOSRootCore
    {
        bool Analyze();
        bool AnalyzeEx(SilentMode silentMode, HiddenMode hiddenMode, WaitAnalysisMode waitMode);
        HashSet<MemberForces> RetrieveMemberForces(IEnumerable<int> beams, IEnumerable<int> loadCases);
    }
}

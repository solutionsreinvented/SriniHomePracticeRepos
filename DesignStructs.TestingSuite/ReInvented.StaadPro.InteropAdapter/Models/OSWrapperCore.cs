using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Interfaces;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSRootCore : IOSRootCore
    {
        public OSRootCore(OpenSTAAD openStaad)
        {
            OpenStaad = openStaad;
        }

        private OpenSTAAD OpenStaad { get; set; }

        public HashSet<MemberForces> RetrieveMemberForces(IEnumerable<int> beams, IEnumerable<int> loadCases)
        {
            return OpenStaad.RetrieveMemberForces(beams, loadCases);
        }

    }

    public class OSWrapperCore
    {
        public OSWrapperCore(OpenStaadWrapper wrapper)
        {
            Wrapper = wrapper;
            OSRootCore = new OSRootCore(Wrapper.OpenStaad);
        }

        public OpenStaadWrapper Wrapper { get; private set; }

        public IOSRootCore OSRootCore { get; private set; }
    }
}

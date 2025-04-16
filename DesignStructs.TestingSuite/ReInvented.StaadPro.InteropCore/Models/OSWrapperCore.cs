using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Models;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSWrapperCore
    {
        public OSWrapperCore(OpenStaadWrapper wrapper)
        {
            Wrapper = wrapper;
            IsDedicated = wrapper != null && wrapper.IsDedicated;

            OpenStaad = new OpenStaadCore(wrapper.OpenStaad);
            Geometry = new OSGeometryCore(wrapper.Geometry);
        }

        public OpenStaadWrapper Wrapper { get; private set; }

        public OpenStaadCore OpenStaad { get; private set; }

        public OSGeometryCore Geometry { get; private set; }

        public OSOutputCore Output { get; private set; }


        public bool IsDedicated { get; private set; }
    }
}

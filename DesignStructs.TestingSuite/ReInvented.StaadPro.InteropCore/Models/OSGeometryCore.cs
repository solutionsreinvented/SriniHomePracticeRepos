
using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSGeometryCore
    {
        public OSGeometryCore(OSGeometryUI geometry)
        {
            ComObject = geometry;
        }

        #region Private Properties

        public OSGeometryUI ComObject { get; private set; }

        #endregion

        public int GetSolidCount() => ComObject.GetSolidCount();

    }
}

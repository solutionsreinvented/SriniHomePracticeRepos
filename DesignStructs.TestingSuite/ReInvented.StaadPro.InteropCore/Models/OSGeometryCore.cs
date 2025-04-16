
using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSGeometryCore
    {
        #region Parameterized Constructor

        public OSGeometryCore(OSGeometryUI geometry)
        {
            ComObject = geometry;
        }

        #endregion

        #region Private Properties

        public OSGeometryUI ComObject { get; private set; }

        #endregion

        #region Public Functions

        public int GetSolidCount() => ComObject.GetSolidCount(); 

        #endregion
    }
}

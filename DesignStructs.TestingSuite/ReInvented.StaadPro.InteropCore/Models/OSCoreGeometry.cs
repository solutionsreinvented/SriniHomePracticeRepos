using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreGeometry
    {
        #region Parameterized Constructor

        public OSCoreGeometry(OSGeometryUI geometry)
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

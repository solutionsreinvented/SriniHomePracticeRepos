using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreSupport
    {
        #region Parameterized Constructor

        public OSCoreSupport(OSSupportUI support)
        {
            ComObject = support;
        }

        #endregion

        #region Private Properties

        public OSSupportUI ComObject { get; private set; }

        #endregion
    }
}

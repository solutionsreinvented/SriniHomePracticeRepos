using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreDesign
    {
        #region Parameterized Constructor

        public OSCoreDesign(OSDesignUI design)
        {
            ComObject = design;
        }

        #endregion

        #region Private Properties

        public OSDesignUI ComObject { get; private set; }

        #endregion
    }
}

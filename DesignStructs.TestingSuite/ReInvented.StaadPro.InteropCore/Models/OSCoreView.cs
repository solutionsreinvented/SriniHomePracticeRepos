using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreView
    {
        #region Parameterized Constructor

        public OSCoreView(OSViewUI view)
        {
            ComObject = view;
        }

        #endregion

        #region Private Properties

        public OSViewUI ComObject { get; private set; }

        #endregion
    }
}

using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreRoot
    {
        #region Parameterized Constructor

        public OSCoreRoot(OpenSTAAD openStaad)
        {
            ComObject = openStaad;
        }

        #endregion

        #region Readonly Properties

        public OpenSTAAD ComObject { get; private set; }

        #endregion

    }
}

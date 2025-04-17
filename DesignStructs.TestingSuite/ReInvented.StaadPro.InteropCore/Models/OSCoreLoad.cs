using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreLoad
    {
        #region Parameterized Constructor

        public OSCoreLoad(OSLoadUI load)
        {
            ComObject = load;
        }

        #endregion

        #region Private Properties

        public OSLoadUI ComObject { get; private set; }

        #endregion

    }
}

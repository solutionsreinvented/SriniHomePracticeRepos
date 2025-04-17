using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreOutput
    {
        #region Parameterized Constructor

        public OSCoreOutput(OSOutputUI output)
        {
            ComObject = output;
        }

        #endregion

        #region Private Properties

        public OSOutputUI ComObject { get; private set; }

        #endregion
    }
}

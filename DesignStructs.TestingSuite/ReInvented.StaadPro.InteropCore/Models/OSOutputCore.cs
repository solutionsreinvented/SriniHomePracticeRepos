
using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSOutputCore
    {
        #region Parameterized Constructor

        public OSOutputCore(OSOutputUI output)
        {
            ComObject = output;
        }

        #endregion

        #region Private Properties

        public OSOutputUI ComObject { get; private set; }

        #endregion
    }
}


using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSLoadCore
    {
        #region Parameterized Constructor

        public OSLoadCore(OSLoadUI load)
        {
            ComObject = load;
        }

        #endregion

        #region Private Properties

        public OSLoadUI ComObject { get; private set; }

        #endregion

    }
}

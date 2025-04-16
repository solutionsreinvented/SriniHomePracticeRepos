
using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OpenStaadCore
    {
        #region Parameterized Constructor

        public OpenStaadCore(OpenSTAAD openStaad)
        {
            ComObject = openStaad;
        }

        #endregion

        #region Readonly Properties

        public OpenSTAAD ComObject { get; private set; }

        #endregion

    }
}

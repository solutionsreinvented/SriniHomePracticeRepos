using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreTable
    {
        #region Parameterized Constructor

        public OSCoreTable(OSTableUI table)
        {
            ComObject = table;
        }

        #endregion

        #region Private Properties

        public OSTableUI ComObject { get; private set; }

        #endregion
    }
}

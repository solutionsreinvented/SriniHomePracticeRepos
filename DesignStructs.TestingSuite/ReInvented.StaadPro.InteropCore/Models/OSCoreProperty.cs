using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreProperty
    {
        #region Parameterized Constructor

        public OSCoreProperty(OSPropertyUI property)
        {
            ComObject = property;
        }

        #endregion

        #region Private Properties

        public OSPropertyUI ComObject { get; private set; }

        #endregion
    }
}

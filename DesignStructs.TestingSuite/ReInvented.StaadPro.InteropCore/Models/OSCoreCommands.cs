using OpenSTAADUI;

namespace ReInvented.StaadPro.InteropCore.Models
{
    public class OSCoreCommands
    {
        #region Parameterized Constructor

        public OSCoreCommands(OSCommandsUI commands)
        {
            ComObject = commands;
        }

        #endregion

        #region Private Properties

        public OSCommandsUI ComObject { get; private set; }

        #endregion
    }
}

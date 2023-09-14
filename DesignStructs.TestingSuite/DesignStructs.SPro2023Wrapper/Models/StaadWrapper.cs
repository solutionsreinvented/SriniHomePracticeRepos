using System.Runtime.InteropServices;

using OpenSTAADUI;

namespace ReInvented.SPro2023Wrapper.Models
{
    public class StaadWrapper
    {
        public StaadWrapper()
        {
            OpenStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;
            Commands = OpenStaad.Command as IOSCommandsUI;
        }

        public OpenSTAAD OpenStaad { get; set; }

        public IOSCommandsUI Commands { get; set; }
    }
}

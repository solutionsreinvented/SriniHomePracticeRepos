using OpenSTAADUI;

using System.Runtime.InteropServices;

namespace SPro2023ConsoleApp
{
    public class StaadWrapper
    {
        public StaadWrapper()
        {
            OpenStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;
            Geometry = OpenStaad.Geometry;
            Load = OpenStaad.Load;
        }

        public OpenSTAAD OpenStaad { get; set; }

        public OSGeometryUI Geometry { get; set; }

        public OSLoadUI Load { get; set; }
    }
}

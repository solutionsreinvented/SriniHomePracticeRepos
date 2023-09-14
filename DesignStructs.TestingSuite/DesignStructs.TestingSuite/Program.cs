using System.Runtime.InteropServices;

using OpenSTAADUI;

namespace ReInvented.TestingSuite
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenSTAAD openStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;
            openStaad.AnalyzeEx(1, 1, 1);
        }
    }
}

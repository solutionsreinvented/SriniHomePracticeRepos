using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using OpenSTAADUI;

using ReInvented.StaadPro.Interactivity.Enums;

namespace ReInvented.TestingSuite
{
    public class StaadQuery
    {
        public static void Run()
        {
            IOpenSTAADUI openStaad = Marshal.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD;

            IOSOutputUI output = openStaad.Output;

            string filePath = @"C:\Users\masanams\Desktop\Desktop\Code\STAAD\D11.3H03.00S09.00OC1.050SC1.128IMP0.439CON0.1665MOT0054.std";

            openStaad.OpenSTAADFile(filePath);

            object fileName = string.Empty;

            do
            {

                openStaad.GetSTAADFile(ref fileName, filePath);
                Thread.Sleep(200);

            } while ((string)fileName != Path.GetFileName(filePath));

            if (!output.AreResultsAvailable())
            {
                var result = (AnalysisResult)openStaad.AnalyzeEx(SilentMode.Enable, HiddenMode.Disable, WaitMode.Wait);
            }


        }
    }
    public enum SilentMode
    {
        Disable = 0,
        Enable = 1
    }
    public enum HiddenMode
    {
        Disable = 0,
        Enable = 1
    }
    public enum WaitMode
    {
        Return = 0,
        Wait = 1
    }
}

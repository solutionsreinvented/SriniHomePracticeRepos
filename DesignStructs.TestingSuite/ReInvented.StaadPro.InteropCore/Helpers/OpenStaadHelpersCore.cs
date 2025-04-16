
using OpenSTAADUI;

using ReInvented.Shared.Helpers;
using ReInvented.Shared.Interfaces;
using ReInvented.Shared.Services;
using ReInvented.StaadPro.Interop.Base;

namespace ReInvented.StaadPro.InteropCore.Helpers
{
    public class OpenStaadHelpersCore : OpenStaadHelpersBase
    {
        #region Public Static Functions

        public static OpenSTAAD GetByStartingStaadApplication(int waitSeconds = 600)
        {
            string applicationPath = @"C:\Program Files\Bentley\Engineering\STAAD.Pro 2024\STAAD\Bentley.Staad.exe";
            IResult<string> processResult = ProcessesService.StartApplication(applicationPath, RegexPatterns.StaadWindowTitle, waitSeconds);

            ///TODO: This line is the only difference between .NET Framework and .NET Core versions
            return processResult.Success ? MarshalHelpers.GetActiveObject("StaadPro.OpenSTAAD") as OpenSTAAD : null;
        }

        #endregion
    }
}

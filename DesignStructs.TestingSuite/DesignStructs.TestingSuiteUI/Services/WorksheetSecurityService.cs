using Excel = Microsoft.Office.Interop.Excel;

namespace DesignStructs.TestingSuiteUI.Services
{
    public class WorksheetSecurityService
    {
        private const string _password = "EngineeredProgramming";

        public static void UnprotectSheet(Excel.Worksheet worksheet)
        {
            worksheet.Unprotect(_password);
        }

        public static void ProtectSheet(Excel.Worksheet worksheet)
        {
            worksheet.Protect(_password);
        }
    }
}

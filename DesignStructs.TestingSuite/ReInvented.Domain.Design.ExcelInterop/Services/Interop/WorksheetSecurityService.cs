using Microsoft.Office.Core;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.Domain.Design.ExcelInterop.Services
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

        public static void SetMacrosSecurityToRunMacros(Excel.Application excelApp)
        {
            MsoAutomationSecurity currentLevel = excelApp.AutomationSecurity;

            if (currentLevel != MsoAutomationSecurity.msoAutomationSecurityLow)
            {
                excelApp.AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityLow;
            }
        }

        public static void ResetMacrosSecurityToDefaultRecommended(Excel.Application excelApp)
        {
            MsoAutomationSecurity currentLevel = excelApp.AutomationSecurity;

            if (currentLevel != MsoAutomationSecurity.msoAutomationSecurityByUI)
            {
                excelApp.AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityByUI;
            }
        }
    }
}

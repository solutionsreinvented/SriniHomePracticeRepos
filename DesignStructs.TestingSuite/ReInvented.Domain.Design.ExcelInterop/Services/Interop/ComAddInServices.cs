using System;

using Microsoft.Office.Core;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.Domain.Design.ExcelInterop.Services
{
    public class ComAddInServices
    {
        public static void ToggleComAddIn(Excel.Application excelApp, bool toggle)
        {
            try
            {
                foreach (COMAddIn addIn in excelApp.COMAddIns)
                {
                    if (addIn.ProgId.Contains("Xtractor"))
                    {
                        addIn.Connect = toggle;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

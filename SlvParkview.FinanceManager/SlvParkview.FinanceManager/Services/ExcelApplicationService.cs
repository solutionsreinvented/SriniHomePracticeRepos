using System;

using Microsoft.Office.Interop.Excel;


namespace SlvParkview.FinanceManager.Services
{
    public static class ExcelApplicationService
    {
        public static Application GetApplication()
        {
            Application xlApplication = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            return xlApplication;
        }

        public static void ReleaseObject(object targetObject)
        {
            try
            {
                _ = System.Runtime.InteropServices.Marshal.ReleaseComObject(targetObject);
                targetObject = null;
            }
            catch (Exception ex)
            {
                targetObject = null;
                _ = System.Windows.MessageBox.Show($"Unable to release the object!\n{ex.Message}");
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

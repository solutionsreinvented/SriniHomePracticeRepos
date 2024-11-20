using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;

namespace ReInvented.Reporting.ExcelInterop.Extensions
{
    public static class ApplicationExtensions
    {
        public static void KillIfOpen(string filePath)
        {
            Process[] xlProcesses = Process.GetProcessesByName("EXCEL").Where(p => p.MainWindowTitle.Contains(Path.GetFileNameWithoutExtension(filePath))).ToArray();
            foreach (Process process in xlProcesses)
            {
                try
                {
                    process.Kill(); // Kill the process that matches the workbook
                }
                catch
                {
                    // Handle any errors gracefully (for example, if access is denied)
                }
            }
        }


        public static void CloseOpenExcelFile(string filePath)
        {
            Application excelApp = null;
            Workbook workbookToClose = null;

            try
            {
                excelApp = new Application() { Visible = true };
                // Iterate through open workbooks to find the one that matches the file path
                foreach (Workbook workbook in excelApp.Workbooks)
                {
                    if (workbook.FullName.Equals(filePath, StringComparison.OrdinalIgnoreCase))
                    {
                        workbookToClose = workbook;
                        break;
                    }
                }

                if (workbookToClose != null)
                {
                    workbookToClose.Close(false); // Discard changes
                                                  // Find the process that corresponds to the Excel instance with the matching workbook
                    Process[] processes = Process.GetProcessesByName("EXCEL");

                    foreach (Process process in processes)
                    {
                        // Iterate through the Excel processes to find the one associated with the workbook
                        foreach (ProcessThread thread in process.Threads)
                        {
                            if (process.MainWindowTitle.Contains(workbookToClose.Name))
                            {
                                try
                                {
                                    process.Kill(); // Kill the process that matches the workbook
                                }
                                catch
                                {
                                    // Handle any errors gracefully (for example, if access is denied)
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while closing the file: {ex.Message}");
            }
            finally
            {
                if (excelApp != null)
                {
                    excelApp.Quit();
                    _ = Marshal.ReleaseComObject(excelApp);
                }

                // Force garbage collection to clean up any remaining COM objects
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}

using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.Sections.Domain.Interfaces;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using ReInvented.Shared.Extensions;
using ReInvented.Shared;
using ReInvented.DataAccess.Services;
using System.IO;
using ReInvented.ExcelInteropDesign.Models;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class ExcelInteropService
    {
        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _wsCalcs;
        private Excel.Worksheet _wsSummary;

        public Dictionary<string, double> DesignBeams(List<IRolledSection> sections, SectionDesignData designData)
        {
            Stopwatch overallTimeStopwatch = new Stopwatch();

            Dictionary<string, double> utilizationRatios = new Dictionary<string, double>();

            overallTimeStopwatch.Start();

            _excelApp = new Excel.Application
            {
                DisplayAlerts = false
            };

            Excel.Dialogs dialogs = _excelApp.Dialogs;

            string filePath = Path.Combine(FileServiceProvider.ExcelTemplatesDirectory, "Beams", "IBeam.xlsm");

            try
            {
                _workbook = _excelApp.Workbooks.Open(filePath);

                _wsCalcs = (Excel.Worksheet)_workbook.Sheets["Calcs"];
                _wsSummary = (Excel.Worksheet)_workbook.Sheets["Summary"];

                #region Fill In Input Data

                ///_wsCalcs.Range["MaterialSpecification"].Value2;
                ///_wsCalcs.Range["MaterialGrade"].Value2;
                ///_wsCalcs.Range["SectionProfile"].Value2;
                ///_wsCalcs.Range["Fy"].Value2;
                ///_wsCalcs.Range["Fu"].Value2;

                #endregion

                WorksheetSecurityService.UnprotectSheet(_wsCalcs);

                Stopwatch designTimeStopwatch = new Stopwatch();

                designTimeStopwatch.Start();

                ///CalculationsSheetService.FillMaterialProperties(_wsCalcs, designData.MaterialTable, designData.MaterialGrade);

                sections.ForEach(s =>
                {
                    _wsCalcs.Range[RangeNames.SectionProfile].Value2 = s.Designation;
                    CalculationsSheetService.FillSectionProperties(_wsCalcs, s);
                    double ur = Convert.ToDouble(_wsSummary.Range[RangeNames.GoverningUtilizationRatio].Value2);

                    utilizationRatios.Add(s.Designation, ur.Ceiling(0.001));
                });

                designTimeStopwatch.Stop();

                _ = MessageBox.Show($"Time took to complete the design of {sections.Count} sections is: {designTimeStopwatch.Elapsed.FormatTime()}");

                WorksheetSecurityService.ProtectSheet(_wsCalcs);

                _workbook.Save();///.SaveAs(filePath, ConflictResolution: Excel.XlSaveConflictResolution.xlLocalSessionChanges);

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_workbook != null)
                {
                    ///ComAddInServices.ToggleComAddIn(_excelApp, false);
                    _workbook.Close();
                    _excelApp.Quit();

                    _ = Marshal.ReleaseComObject(_wsCalcs);
                    _ = Marshal.ReleaseComObject(_workbook);
                }

                _ = Marshal.ReleaseComObject(_excelApp);

            }

            overallTimeStopwatch.Stop();

            _ = MessageBox.Show($"Time took to complete the entire process is: {overallTimeStopwatch.Elapsed.FormatTime()}");


            return utilizationRatios;
        }
    }
}

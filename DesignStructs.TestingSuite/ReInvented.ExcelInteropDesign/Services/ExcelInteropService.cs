using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.Sections.Domain.Interfaces;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class ExcelInteropService
    {
        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _wsCals;
        private Excel.Worksheet _wsSummary;

        public Dictionary<string, double> VerifyDesign(List<IRolledSection> sections)
        {
            Dictionary<string, double> utilizationRatios = new Dictionary<string, double>();

            _excelApp = new Excel.Application
            {
                DisplayAlerts = false
            };

            Excel.Dialogs dialogs = _excelApp.Dialogs;

            string filePath = @"C:\Users\masanams\Desktop\Radial Beams.xlsm";

            try
            {
                _workbook = _excelApp.Workbooks.Open(filePath);

                _wsCals = (Excel.Worksheet)_workbook.Sheets["Calcs"];
                _wsSummary = (Excel.Worksheet)_workbook.Sheets["Summary"];

                Excel.Range rngSectionProperties = _wsCals.Range["SectionProperties"];

                WorksheetSecurityService.UnprotectSheet(_wsCals);


                sections.ForEach(s =>
                {
                    rngSectionProperties.ClearContents();
                    SectionPropertiesService.Instance.FillISectionPropertiesInSpreadSheet(_wsCals, s as RolledSectionHShape, rngSectionProperties.Row, rngSectionProperties.Column);
                    utilizationRatios.Add(s.Designation, Convert.ToDouble(_wsSummary.Range["MaxUR"].Value2).Ceiling(0.001));
                });


                WorksheetSecurityService.ProtectSheet(_wsCals);

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

                    _ = Marshal.ReleaseComObject(_wsCals);
                    _ = Marshal.ReleaseComObject(_workbook);
                }

                _ = Marshal.ReleaseComObject(_excelApp);

            }

            return utilizationRatios;
        }
    }
}

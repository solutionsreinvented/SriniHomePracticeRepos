﻿using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

using ReInvented.Sections.Domain.Interfaces;
using System;
using System.Windows;
using System.Runtime.InteropServices;
using ReInvented.Sections.Domain.Models;
using System.Timers;
using System.Diagnostics;
using ReInvented.Shared.Extensions;
using ReInvented.Shared;

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
            Stopwatch overallTimeStopwatch = new Stopwatch();

            Dictionary<string, double> utilizationRatios = new Dictionary<string, double>();

            overallTimeStopwatch.Start();

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

                Stopwatch designTimeStopwatch = new Stopwatch();

                designTimeStopwatch.Start();

                sections.ForEach(s =>
                {
                    rngSectionProperties.ClearContents();
                    SectionPropertiesService.Instance.FillISectionPropertiesInSpreadSheet(_wsCals, s as RolledSectionHShape, rngSectionProperties.Row, rngSectionProperties.Column);
                    double ur = Convert.ToDouble(_wsSummary.Range["MaxUR"].Value2);
                    
                    utilizationRatios.Add(s.Designation, ur.Ceiling(0.001));
                });

                designTimeStopwatch.Stop();

                _ = MessageBox.Show($"Time took to complete the design of {sections.Count} sections is: {designTimeStopwatch.Elapsed.FormatTime()}");

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

            overallTimeStopwatch.Stop();

            _ = MessageBox.Show($"Time took to complete the entire process is: {overallTimeStopwatch.Elapsed.FormatTime()}");


            return utilizationRatios;
        }
    }
}

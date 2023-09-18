using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Base;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Shared;
using ReInvented.Shared.Extensions;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInteropDesign.Services.Design
{
    public class GenericSectionDesignService<TSection> where TSection : RolledSection, IRolledSection
    {
        #region Private Fields

        private static GenericSectionDesignService<TSection> _instance;

        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _wsCalcs;
        private Excel.Worksheet _wsSummary;

        #endregion

        #region Private Constructor

        private GenericSectionDesignService()
        {

        } 

        #endregion

        #region Instance Provider

        public static GenericSectionDesignService<TSection> Instance
        {
            get
            {
                return _instance ?? (_instance = new GenericSectionDesignService<TSection>());
            }
        } 

        #endregion

        public Dictionary<string, double> Design(List<TSection> sections, string fullFilePath, Action<Excel.Worksheet, IRolledSection> fillSectionProperties)
        {
            Stopwatch overallTimeStopwatch = new Stopwatch();

            Dictionary<string, double> utilizationRatios = new Dictionary<string, double>();

            overallTimeStopwatch.Start();

            _excelApp = new Excel.Application
            {
                DisplayAlerts = false
            };

            Excel.Dialogs dialogs = _excelApp.Dialogs;

            try
            {
                _workbook = _excelApp.Workbooks.Open(fullFilePath);
                _wsCalcs = (Excel.Worksheet)_workbook.Sheets["Calcs"];
                _wsSummary = (Excel.Worksheet)_workbook.Sheets["Summary"];

                WorksheetSecurityService.UnprotectSheet(_wsCalcs);

                Stopwatch designTimeStopwatch = new Stopwatch();

                designTimeStopwatch.Start();

                ///SummarySheetService.FillForcesInSummaryTable(_wsSummary, (new SectionDesignData()).ForcesSummary);
                ///CalculationsSheetService.FillMaterialProperties(_wsCalcs, designData.MaterialTable, designData.MaterialGrade);
                ///CalculationsSheetService.FillAxialStrengthParameters(_wsCalcs, (new SectionDesignData()).AxialStrengthParameters);
                ///CalculationsSheetService.FillStiffenersParameters(_wsCalcs, (new SectionDesignData()).Stiffeners);
                ///CalculationsSheetService.FillMethodOfDesign(_wsCalcs, (new SectionDesignData()).DesignMethod);


                sections.ForEach(s =>
                {
                    _wsCalcs.Range[RangeNames.SectionProfile].Value2 = s.Designation;
                    ///CalculationsSheetService.FillISectionProperties(_wsCalcs, s);
                    fillSectionProperties(_wsCalcs, s);
                    double ur = Convert.ToDouble(_wsSummary.Range[RangeNames.GoverningUtilizationRatio].Value2);

                    utilizationRatios.Add(s.Designation, ur.Ceiling(0.001));
                });

                designTimeStopwatch.Stop();

                _ = MessageBox.Show($"Time took to complete the design of {sections.Count} sections is: {designTimeStopwatch.Elapsed.FormatTime()}");

                WorksheetSecurityService.ProtectSheet(_wsCalcs);

                _workbook.Save();

            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
            finally
            {
                if (_workbook != null)
                {
                    _workbook.Close();
                    _excelApp.Quit();

                    _ = Marshal.ReleaseComObject(_wsSummary);
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

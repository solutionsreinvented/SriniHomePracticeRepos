using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

using ReInvented.ExcelInteropDesign.Factories;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Base;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Shared;
using ReInvented.Shared.Extensions;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class SectionDesignExcelInteropService<TSection> where TSection : RolledSection, IRolledSection
    {
        #region Private Fields

        private static SectionDesignExcelInteropService<TSection> _instance;

        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _wsCalcs;
        private Excel.Worksheet _wsSummary;

        #endregion

        #region Private Constructor

        private SectionDesignExcelInteropService()
        {

        }

        #endregion

        #region Instance Provider

        public static SectionDesignExcelInteropService<TSection> Instance
        { 
            get
            {
                return _instance ?? (_instance = new SectionDesignExcelInteropService<TSection>());
            }
        }

        #endregion

        #region Public Functions

        public Dictionary<string, double> Design(IEnumerable<TSection> sections, ISectionDesignData designData, string fullFilePath)
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

                SummarySheetService.FillForcesInSummaryTable(_wsSummary, designData.ForcesSummary);

                ICalculationSheetService calculationSheetService = CalculationSheetServiceFactory.Get<TSection>();

                RolledSectionKey parametersKey = new RolledSectionKey(typeof(TSection));

                calculationSheetService.FillMaterialProperties(_wsCalcs, designData.MaterialTable, designData.MaterialGrade);
                calculationSheetService.FillAxialStrengthParameters(_wsCalcs, designData.AxialStrengthParametersDictionary[parametersKey]);
                calculationSheetService.FillShearStrengthParameters(_wsCalcs, designData.ShearStrengthParametersDictionary[parametersKey]);
                calculationSheetService.FillMethodOfDesign(_wsCalcs, designData.DesignMethod);


                sections.ToList().ForEach(s =>
                {
                    _wsCalcs.Range[RangeNames.SectionProfile].Value2 = s.Designation;
                    calculationSheetService.FillSectionProperties(_wsCalcs, s);
                    double ur = Convert.ToDouble(_wsSummary.Range[RangeNames.GoverningUtilizationRatio].Value2);

                    utilizationRatios.Add(s.Designation, ur.Ceiling(0.001));
                });

                designTimeStopwatch.Stop();

                _ = MessageBox.Show($"Time took to complete the design of {sections.Count()} sections is: {designTimeStopwatch.Elapsed.FormatTime()}");

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

        #endregion
    }
}

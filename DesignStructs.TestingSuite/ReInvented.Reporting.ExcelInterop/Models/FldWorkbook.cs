using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;
using ReInvented.Domain.Reporting.Models;
using ReInvented.Reporting.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Entities;
using System.Diagnostics;
using ReInvented.Reporting.ExcelInterop.Services;

namespace ReInvented.Reporting.ExcelInterop.Models
{
    public class FldWorkbook : IDisposable
    {
        #region Private Static Fields

        private const string _inputBackground = "#F8EEC8";
        private const string _highlight = "#F5E6B1";

        #endregion

        #region Parameterized Constructor

        public FldWorkbook(string savePath, string fileName, FLDReport fldReport)
        {
            if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Either {nameof(savePath)} or {nameof(fileName)} is invalid. Please provide valid data.");
            }

            if (!Directory.Exists(savePath))
            {
                _ = Directory.CreateDirectory(savePath);
            }

            FldReport = fldReport;
            SavePath = savePath;
            FileName = fileName;
        }

        #endregion

        #region Public Properties

        public FLDReport FldReport { get; private set; }

        public string SavePath { get; private set; }

        public string FileName { get; private set; }

        public Application App { get; set; }

        public Workbook Workbook { get; private set; }

        public Worksheet Worksheet { get; set; }

        #endregion

        #region Instance Methods

        public void InstantiateObjects()
        {
            ///DoRequiredWithWebView();
            ApplicationExtensions.KillIfOpen(Path.Combine(SavePath, FileName));
            if (App == null)
            {
                App = new Application { Visible = true, DisplayAlerts = false };
            }
            if (Workbook == null)
            {
                Workbook = App.Workbooks.Add();
            }
            if (Worksheet == null)
            {
                Worksheet = (Worksheet)Workbook.Sheets[1];
            }
        }

        public void Create()
        {
            try
            {
                InstantiateObjects();

                Worksheet.Name = "Exported Data";
                Workbook.SaveAs(Path.Combine(SavePath, FileName), XlFileFormat.xlOpenXMLWorkbookMacroEnabled);


                DocumentHeaderService.CreateDocumentHeader(Worksheet.SetTemplateDefaults(), _highlight);

                /* Generate foundation load data tables from FldReport */

                if (FldReport != null)
                {
                    FontSettings headerFont = XlSettings.HeaderDefaultFont;
                    int currentRow = XlSettings.ContentStartRow;
                    int sectionId = 1;
                    int nRowsHeader = 2;

                    FoundationLoadData fld = FldReport.Content as FoundationLoadData;
                    HashSet<LoadCaseForces> overallSummary = fld.OverallSummary;
                    HashSet<PCDLoads> pcdForcesCollection = fld.PCDLoadsCollection;

                    DocumentHeaderService.FillDocumentHeaderData(Worksheet, FldReport.ProjectData, FldReport.Document);
                    Worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}. All Supports");
                    Worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.1 Summary of Loads from All Supports (Statics Check)");

                    Worksheet.Application.ActiveWindow.DisplayGridlines = false;

                    currentRow = SummaryTableService.Generate(Worksheet, currentRow, nRowsHeader, overallSummary, fld.LoadCases);

                    foreach (PCDLoads pcdForces in pcdForcesCollection)
                    {
                        currentRow += XlSettings.HeadersOffset;

                        sectionId++;
                        string pcdDesc = pcdForces.PCD == "CC" ? "Center Column" : pcdForces.PCD;
                        Worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}. {pcdDesc} Supports");

                        Worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.1 Supports Information");
                        currentRow = SupportInformationTableService.Generate(Worksheet, ++currentRow, pcdForces.SupportsInformation);

                        currentRow += XlSettings.HeadersOffset;
                        Worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.2 Summary of Reactions at Support Group C.G. ({pcdDesc})");
                        currentRow = SummaryTableService.Generate(Worksheet, currentRow, nRowsHeader, pcdForces.SupportLoadsSummary, fld.LoadCases);

                        currentRow += XlSettings.HeadersOffset;
                        Worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.3 Reactions at Each Support");


                        currentRow += XlSettings.HeadersOffset;
                        currentRow = PcdLoadsTableService.GenerateHeaders(Worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases);

                        foreach (SupportLoads sLoads in pcdForces.SupportLoadsCollection)
                        {
                            currentRow = PcdLoadsTableService.GenerateSupportLoadsRow(Worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases, sLoads, fld.LoadCases);
                        }

                    }

                }

                Workbook.Save();
            }
            catch (Exception ex)
            {
                ///TODO: Replace this with a log file
                Debug.Print(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            // Cleanup resources

            if (Worksheet != null)
            {
                Marshal.ReleaseComObject(Worksheet);
                Worksheet = null;
            }
            if (Workbook != null)
            {
                //Workbook.Close(false);
                Marshal.ReleaseComObject(Workbook);
                Workbook = null;
            }

            if (App != null)
            {
                App.Quit();
                Marshal.ReleaseComObject(App);
                App = null;
            }

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

    }
}

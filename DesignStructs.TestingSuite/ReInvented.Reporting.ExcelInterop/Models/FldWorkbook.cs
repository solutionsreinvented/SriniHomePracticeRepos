using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;
using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using System.Diagnostics;
using ReInvented.Reporting.ExcelInterop.Services;
using ReInvented.ExcelInterop.Extensions;
using HtmlAgilityPack;
using System.Linq;
using ReInvented.ExcelInterop.Models;
using ReInvented.Shared.Extensions;
using ReInvented.Shared.Services;

namespace ReInvented.Reporting.ExcelInterop.Models
{
    public class FldWorkbook : IDisposable
    {
        #region Private Static Fields

        private const string _inputBackground = "#F8EEC8";
        private const string _highlight = "#F5E6B1";

        #endregion

        #region Parameterized Constructor

        public FldWorkbook(string savePath, string fileName, FLDReport fldReport, HtmlDocument htmlReport)
        {
            Initialize(savePath, fileName, fldReport, htmlReport);
        }

        #endregion

        #region Public Properties

        public string SavePath { get; private set; }

        public string FileName { get; private set; }

        public FLDReport FldReport { get; private set; }

        public HtmlDocument HtmlReport { get; private set; }

        public Application App { get; set; }

        public Workbook Workbook { get; private set; }

        public Worksheet Worksheet { get; set; }

        #endregion

        #region Instance Methods

        public void InstantiateObjects()
        {
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

                if (FldReport != null)
                {
                    FontSettings headerFont = XlSettings.HeaderDefaultFont;
                    int currentRow = XlSettings.ContentStartRow;
                    int sectionId = 1;
                    int nRowsHeader = 2;

                    Worksheet.Application.ActiveWindow.DisplayGridlines = false;

                    IEnumerable<HtmlNode> svgNodes = HtmlReport.GetAllNodesBy("svg");
                    FoundationLoadData fld = FldReport.Content as FoundationLoadData;
                    HashSet<LoadCaseForces> overallSummary = fld.OverallSummary;
                    HashSet<PCDLoads> pcdForcesCollection = fld.PCDLoadsCollection;

                    currentRow = GenerateSummaryForAllSupports(Worksheet, FldReport, overallSummary, currentRow, sectionId, nRowsHeader, fld.LoadCases, headerFont);

                    foreach (PCDLoads pcdForces in pcdForcesCollection)
                    {
                        currentRow += XlSettings.HeadersOffset;
                        sectionId++;

                        currentRow = GeneratePcdMainHeadingAndSupportsInformation(Worksheet, pcdForces, currentRow, sectionId, headerFont);
                        currentRow = GenerateSupportLayoutImage(Worksheet, currentRow, pcdForces, svgNodes);
                        currentRow = GeneratePcdLoadsSummaryAtCg(Worksheet, pcdForces, fld.LoadCases, currentRow, sectionId, nRowsHeader, headerFont);
                        currentRow = GenerateReactionsAtEachSupport(Worksheet, currentRow, headerFont, sectionId, pcdForces, fld.LoadCases);
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

        #region Private Helpers

        private void Initialize(string savePath, string fileName, FLDReport fldReport, HtmlDocument htmlReport)
        {
            if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Either {nameof(savePath)} or {nameof(fileName)} is invalid. Please provide valid data.");
            }

            if (!Directory.Exists(savePath))
            {
                _ = Directory.CreateDirectory(savePath);
            }

            SavePath = savePath;
            FileName = fileName;

            FldReport = fldReport ?? throw new ArgumentNullException($"The {nameof(fldReport)} shall not be null. Please provide valid data.");
            HtmlReport = htmlReport ?? throw new ArgumentNullException($"The {nameof(htmlReport)} shall not be null. Please provide valid data.");
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

        #region Private Static Helpers

        private static int GenerateSummaryForAllSupports(Worksheet worksheet, FLDReport fldReport, HashSet<LoadCaseForces> overallSummary, int currentRow, int sectionId, int nRowsHeader, Dictionary<int, string> loadCases, FontSettings headerFont)
        {
            DocumentHeaderService.FillDocumentHeaderData(worksheet, fldReport.ProjectData, fldReport.Document);
            _ = worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}. All Supports");
            _ = worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.1 Summary of Loads from All Supports (Statics Check)");

            currentRow = SummaryTableService.Generate(worksheet, currentRow, nRowsHeader, overallSummary, loadCases);
            return currentRow;
        }

        private static int GeneratePcdMainHeadingAndSupportsInformation(Worksheet worksheet, PCDLoads pcdForces, int currentRow, int sectionId, FontSettings headerFont)
        {
            string pcdDesc = pcdForces.PCD == "CC" ? "Center Column" : pcdForces.PCD;
            _ = worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}. {pcdDesc} Supports");

            _ = worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.1 Supports Information");
            currentRow = SupportInformationTableService.Generate(worksheet, ++currentRow, pcdForces.SupportsInformation);

            return currentRow;
        }

        private static int GenerateSupportLayoutImage(Worksheet worksheet, int currentRow, PCDLoads pcdForces, IEnumerable<HtmlNode> svgNodes)
        {
            int sColImage = XlSettings.StartColTable;
            int eColImage = XlSettings.EndColTable;
            int rowSpanImage = eColImage - sColImage;

            currentRow += XlSettings.HeadersOffset;
            HtmlNode svg = svgNodes.FirstOrDefault(node => node.GetId() == $"supportLayout{pcdForces.PCD}");

            byte[] svgBytes = SvgToPngConversionService.ConvertSvgToPng(svg.OuterHtml, XlSettings.PngWidth, XlSettings.PngHeight);
            int eRowSvg = currentRow + rowSpanImage;

            _ = worksheet.EmbedPngToExcelFromMemory(svgBytes, worksheet.Range(currentRow, sColImage), worksheet.Range(eRowSvg, eColImage));

            return eRowSvg;
        }

        private static int GeneratePcdLoadsSummaryAtCg(Worksheet worksheet, PCDLoads pcdForces, Dictionary<int, string> loadCases, int currentRow, int sectionId, int nRowsHeader, FontSettings headerFont)
        {
            string pcdDesc = pcdForces.PCD == "CC" ? "Center Column" : pcdForces.PCD;

            currentRow += XlSettings.HeadersOffset;
            _ = worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.2 Summary of Reactions at Support Group C.G. ({pcdDesc})");
            currentRow = SummaryTableService.Generate(worksheet, currentRow, nRowsHeader, pcdForces.SupportLoadsSummary, loadCases);

            return currentRow;
        }

        private static int GenerateReactionsAtEachSupport(Worksheet worksheet, int currentRow, FontSettings headerFont, int sectionId, PCDLoads pcdForces, Dictionary<int, string> loadCases)
        {
            currentRow += XlSettings.HeadersOffset;
            _ = worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle(headerFont).Fill($"{sectionId}.3 Reactions at Each Support");

            currentRow += XlSettings.HeadersOffset;
            currentRow = PcdLoadsTableService.GenerateHeaders(worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases);

            foreach (SupportLoads sLoads in pcdForces.SupportLoadsCollection)
            {
                currentRow = PcdLoadsTableService.GenerateSupportLoadsRow(worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases, sLoads, loadCases);
            }

            return currentRow;
        }

        #endregion
    }
}

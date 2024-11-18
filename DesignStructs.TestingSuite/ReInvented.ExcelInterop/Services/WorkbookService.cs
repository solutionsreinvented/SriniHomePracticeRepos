using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;
using ReInvented.Domain.Reporting.Models;
using ReInvented.ExcelInterop.Extensions;
using ReInvented.StaadPro.Interactivity.Entities;
using System.Linq;
using ReInvented.ExcelInterop.Models;

namespace ReInvented.ExcelInterop.Services
{
    public class WorkbookService
    {
        #region Private Static Fields

        private const string _inputBackground = "#F8EEC8";
        private const string _highlight = "#F5E6B1";

        #endregion

        #region Parameterized Constructor

        public WorkbookService(string savePath, string fileName, FoundationLoadData foundationLoadData)
        {
            if (string.IsNullOrWhiteSpace(savePath) || string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException($"Either {nameof(savePath)} or {nameof(fileName)} is invalid. Please provide valid data.");
            }

            if (!Directory.Exists(savePath))
            {
                _ = Directory.CreateDirectory(savePath);
            }

            FoundationLoadData = foundationLoadData;
            SavePath = savePath;
            FileName = fileName;
        }

        #endregion

        #region Public Properties

        public FoundationLoadData FoundationLoadData { get; private set; }

        public string SavePath { get; private set; }

        public string FileName { get; private set; }

        //public Application Application { get; set; }

        public Workbook Workbook { get; private set; }

        //public Worksheet Worksheet { get; set; }

        #endregion

        #region Instance Methods

        public Workbook Create()
        {
            Workbook = Create(SavePath, FileName);
            return Workbook;
        }

        public void Dispose()
        {
            Workbook.Close(false);
            //Application.Quit();

            //_ = Marshal.ReleaseComObject(Worksheet);
            _ = Marshal.ReleaseComObject(Workbook);
            //_ = Marshal.ReleaseComObject(Application);
        }

        #endregion



        public static Workbook Create(string savePath, string fileName, FLDReport fldReport = null)
        {
            Application excelApp = new Application { Visible = true };

            Workbook workbook = excelApp.Workbooks.Add();
            workbook.SaveAs(Path.Combine(savePath, fileName), XlFileFormat.xlOpenXMLWorkbookMacroEnabled);

            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            worksheet.Name = "Exported Data";

            DocumentHeaderService.CreateDocumentHeader(worksheet.SetTemplateDefaults(), _highlight);


            /* Generate foundation load data tables from fldReport */

            if (fldReport != null)
            {
                int currentRow = XlSettings.ContentStartRow;
                int sectionId = 1;
                int nRowsHeader = 2;

                FoundationLoadData fld = fldReport.Content as FoundationLoadData;
                HashSet<LoadCaseForces> overallSummary = fld.OverallSummary;
                HashSet<PCDLoads> pcdForcesCollection = fld.PCDLoadsCollection;

                DocumentHeaderService.FillDocumentHeaderData(worksheet, fldReport.ProjectData, fldReport.Document);
                worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}. All Supports");
                worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}.1 Summary of Loads from All Supports (Statics Check)");

                worksheet.Application.ActiveWindow.DisplayGridlines = false;

                currentRow = SummaryTableService.Generate(worksheet, currentRow, nRowsHeader, overallSummary, fld.LoadCases);

                foreach (PCDLoads pcdForces in pcdForcesCollection)
                {
                    currentRow += XlSettings.HeadersOffset;

                    sectionId++;
                    string pcdDesc = pcdForces.PCD == "CC" ? "Center Column" : pcdForces.PCD;
                    worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}. {pcdDesc} Supports");

                    worksheet.Range(++currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}.1 Supports Information");
                    currentRow = SupportInformationTableService.Generate(worksheet, ++currentRow, pcdForces.SupportsInformation);

                    currentRow += XlSettings.HeadersOffset;
                    worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}.2 Summary of Reactions at Support Group C.G. ({pcdDesc})");
                    currentRow = SummaryTableService.Generate(worksheet, currentRow, nRowsHeader, pcdForces.SupportLoadsSummary, fld.LoadCases);

                    currentRow += XlSettings.HeadersOffset;
                    worksheet.Range(currentRow, XlSettings.StartColTable).AlignLeftIndented(0).FontStyle("Tahoma", 8, true, false).Fill($"{sectionId}.3 Reactions at Each Support");


                    currentRow += XlSettings.HeadersOffset;
                    currentRow = PcdLoadsTableService.GenerateHeaders(worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases);

                    foreach (SupportLoads sLoads in pcdForces.SupportLoadsCollection)
                    {
                        currentRow = PcdLoadsTableService.GenerateSupportLoadsRow(worksheet, currentRow, pcdForces.SupportsInformation.SupportReleases, sLoads, fld.LoadCases);
                    }



                }

            }

            //string rngTarget = "A10";
            //worksheet.Range(rngTarget).MergeEx().Fill("This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content. This is a sample text that will be wrapped and the row height will adjust automatically to fit the content.").Wrap(15);
















            workbook.Save();

            return workbook;
        }
    }
}

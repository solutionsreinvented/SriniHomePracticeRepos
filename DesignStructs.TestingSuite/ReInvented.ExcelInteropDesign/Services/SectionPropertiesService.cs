using System;
using System.Linq;

using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;
using ReInvented.Shared;

using Excel = Microsoft.Office.Interop.Excel;


namespace ReInvented.ExcelInteropDesign.Services
{
    public class SectionPropertiesService
    {
        #region Private Fields

        private static SectionPropertiesService _instance;

        #endregion

        #region Private Constructor

        private SectionPropertiesService()
        {
            SectionsLibrary = SectionsRepository.Instance.GetSectionsLibrary();
        }

        #endregion

        #region Instance Provider

        public static SectionPropertiesService Instance => _instance ?? (_instance = new SectionPropertiesService());

        #endregion

        #region Private Properties

        private SectionsLibrary SectionsLibrary { get; set; }

        #endregion

        public IRolledSection GetProperties(string dbName, string shape, string sectionClassification, string sectionDesignation)
        {
            IRolledSection section = SectionsLibrary.Databases.FirstOrDefault(d => d.Name == dbName)
                                                    .SectionShapes.FirstOrDefault(s => s.Shape == shape)
                                                    .Classifications.FirstOrDefault(c => c.Category == sectionClassification)
                                                    .Sections.FirstOrDefault(s => s.Designation == sectionDesignation);

            return section;
        }

        public void FillISectionPropertiesInSpreadSheet(Excel.Worksheet worksheet, RolledSectionHShape section, int sRow, int sCol)
        {
            worksheet.Cells[sRow + 0, sCol].Value = section.Designation;
            worksheet.Cells[sRow + 1, sCol].Value = section.Mass;
            worksheet.Cells[sRow + 2, sCol].Value = section.MassFps;
            worksheet.Cells[sRow + 3, sCol].Value = section.H;
            worksheet.Cells[sRow + 4, sCol].Value = section.Bf;
            worksheet.Cells[sRow + 5, sCol].Value = section.Tw;
            worksheet.Cells[sRow + 6, sCol].Value = section.Tf;

            worksheet.Cells[sRow + 7, sCol].Value = section.R1;
            worksheet.Cells[sRow + 8, sCol].Value = section.R2;
            worksheet.Cells[sRow + 9, sCol].Value = section.A * 10.Squared();
            worksheet.Cells[sRow + 10, sCol].Value = section.Hi;
            worksheet.Cells[sRow + 11, sCol].Value = section.D;
            ///worksheet.Cells[sRow + 12, sCol].Value = section.Phi;
            ///worksheet.Cells[sRow + 13, sCol].Value = section.Pmin;
            ///worksheet.Cells[sRow + 14, sCol].Value = section.Pmax;
            worksheet.Cells[sRow + 15, sCol].Value = section.ALO;
            ///worksheet.Cells[sRow + 16, sCol].Value = section.ALI * 10E6;
            worksheet.Cells[sRow + 17, sCol].Value = section.AGO;
            ///worksheet.Cells[sRow + 18, sCol].Value = section.AGI * 10E6;
            worksheet.Cells[sRow + 19, sCol].Value = section.Iy * 10.RestTo(4);
            worksheet.Cells[sRow + 20, sCol].Value = section.Wely * 10.Cubed();
            worksheet.Cells[sRow + 21, sCol].Value = section.Wply * 10.Cubed();
            worksheet.Cells[sRow + 22, sCol].Value = section.Ry * 10;
            worksheet.Cells[sRow + 23, sCol].Value = section.Avz * 10.Squared();
            worksheet.Cells[sRow + 24, sCol].Value = section.Iz * 10.RestTo(4);
            worksheet.Cells[sRow + 25, sCol].Value = section.Welz * 10.Cubed();
            worksheet.Cells[sRow + 26, sCol].Value = section.Wplz * 10.Cubed();
            worksheet.Cells[sRow + 27, sCol].Value = section.Rz * 10;
            worksheet.Cells[sRow + 28, sCol].Value = section.Avy * 10.Squared();
            worksheet.Cells[sRow + 29, sCol].Value = section.Ss;
            worksheet.Cells[sRow + 30, sCol].Value = section.It * 10.RestTo(4);
            worksheet.Cells[sRow + 31, sCol].Value = section.Iw * 10.Cubed() * 10.RestTo(6); /// TODO: The multipliers seems to be incorrect and confusing. Check
            worksheet.Cells[sRow + 32, sCol].Value = section.Alpha;

            ///worksheet.Cells[sRow + 33, sCol].Value = section.K;
            ///worksheet.Cells[sRow + 34, sCol].Value = section.K1;
            worksheet.Cells[sRow + 35, sCol].Value = section.Cy;
            worksheet.Cells[sRow + 36, sCol].Value = section.Ey;
            worksheet.Cells[sRow + 37, sCol].Value = section.Cz;
            worksheet.Cells[sRow + 38, sCol].Value = section.Ez;
        }
    }
}

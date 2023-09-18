﻿using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Shared;

using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class CalculationsSheetService
    {
        public static void FillMaterialProperties(Excel.Worksheet wsCalcs, MaterialTable table, MaterialGrade grade)
        {
            wsCalcs.Range[RangeNames.MaterialSpecification].Value2 = table.Name;
            wsCalcs.Range[RangeNames.MaterialGrade].Value2 = grade.Designation;
            wsCalcs.Range[RangeNames.Fy].Value2 = grade.Fy;
            wsCalcs.Range[RangeNames.Fu].Value2 = grade.Fu;
        }

        public static void FillAxialStrengthParameters(Excel.Worksheet wsCalcs, AxialStrengthParameters parameters)
        {
            Excel.Range rngAxialStrengthParameters = wsCalcs.Range[RangeNames.AxialStrength];

            int sRow = rngAxialStrengthParameters.Row;
            int sColumn = rngAxialStrengthParameters.Column;

            rngAxialStrengthParameters[sRow + 0, sColumn] = parameters.PercentNetTensileArea;
            rngAxialStrengthParameters[sRow + 1, sColumn] = parameters.LateralUnsupportedLength;
        }

        public static void FillStiffenersParameters(Excel.Worksheet wsCalcs, WebTransverseStiffeners stiffeners)
        {
            Excel.Range rngStiffenersParameters = wsCalcs.Range[RangeNames.WebTransverseStiffeners];

            int sRow = rngStiffenersParameters.Row;
            int sColumn = rngStiffenersParameters.Column;

            rngStiffenersParameters[sRow + 0, sColumn] = stiffeners.Configuration == WebTransverseStiffenersConfiguration.None ? "No" : "Yes";
            rngStiffenersParameters[sRow + 1, sColumn] = stiffeners.Configuration == WebTransverseStiffenersConfiguration.BothSides ?
                                                         WebTransverseStiffenersConfiguration.BothSides.GetDescription() :
                                                         WebTransverseStiffenersConfiguration.OneSide.GetDescription();

            rngStiffenersParameters[sRow + 2, sColumn] = stiffeners.Thickness;
            rngStiffenersParameters[sRow + 3, sColumn] = stiffeners.Spacing;
        }

        public static void FillMethodOfDesign(Excel.Worksheet wsCalcs, DesignMethod designMethod)
        {
            wsCalcs.Range[RangeNames.DesignMethod].Value2 = designMethod.GetDescription();
        }

        public static void FillISectionProperties(Excel.Worksheet wsCalcs, IRolledSection sectionProfile)
        {
            RolledSectionHShape section = sectionProfile as RolledSectionHShape;

            Excel.Range rngSectionProperties = wsCalcs.Range[RangeNames.SectionProperties];
            int sRow = rngSectionProperties.Row;
            int sColumn = rngSectionProperties.Column;

            rngSectionProperties.ClearContents();

            wsCalcs.Cells[sRow + 00, sColumn].Value = section.Designation;
            wsCalcs.Cells[sRow + 01, sColumn].Value = section.Mass;
            wsCalcs.Cells[sRow + 02, sColumn].Value = section.MassFps;
            wsCalcs.Cells[sRow + 03, sColumn].Value = section.H;
            wsCalcs.Cells[sRow + 04, sColumn].Value = section.Bf;
            wsCalcs.Cells[sRow + 05, sColumn].Value = section.Tw;
            wsCalcs.Cells[sRow + 06, sColumn].Value = section.Tf;

            wsCalcs.Cells[sRow + 07, sColumn].Value = section.R1;
            wsCalcs.Cells[sRow + 08, sColumn].Value = section.R2;
            wsCalcs.Cells[sRow + 09, sColumn].Value = section.A * 10.Squared();
            wsCalcs.Cells[sRow + 10, sColumn].Value = section.Hi;
            wsCalcs.Cells[sRow + 11, sColumn].Value = section.D;
            ///worksheet.Cells[sRow + 12, sCol].Value = section.Phi;
            ///worksheet.Cells[sRow + 13, sCol].Value = section.Pmin;
            ///worksheet.Cells[sRow + 14, sCol].Value = section.Pmax;
            wsCalcs.Cells[sRow + 15, sColumn].Value = section.ALO;
            ///worksheet.Cells[sRow + 16, sCol].Value = section.ALI * 10E6;
            wsCalcs.Cells[sRow + 17, sColumn].Value = section.AGO;
            ///worksheet.Cells[sRow + 18, sCol].Value = section.AGI * 10E6;
            wsCalcs.Cells[sRow + 19, sColumn].Value = section.Iy * 10.RestTo(4);
            wsCalcs.Cells[sRow + 20, sColumn].Value = section.Wely * 10.Cubed();
            wsCalcs.Cells[sRow + 21, sColumn].Value = section.Wply * 10.Cubed();
            wsCalcs.Cells[sRow + 22, sColumn].Value = section.Ry * 10;
            wsCalcs.Cells[sRow + 23, sColumn].Value = section.Avz * 10.Squared();
            wsCalcs.Cells[sRow + 24, sColumn].Value = section.Iz * 10.RestTo(4);
            wsCalcs.Cells[sRow + 25, sColumn].Value = section.Welz * 10.Cubed();
            wsCalcs.Cells[sRow + 26, sColumn].Value = section.Wplz * 10.Cubed();
            wsCalcs.Cells[sRow + 27, sColumn].Value = section.Rz * 10;
            wsCalcs.Cells[sRow + 28, sColumn].Value = section.Avy * 10.Squared();
            wsCalcs.Cells[sRow + 29, sColumn].Value = section.Ss;
            wsCalcs.Cells[sRow + 30, sColumn].Value = section.It * 10.RestTo(4);
            wsCalcs.Cells[sRow + 31, sColumn].Value = section.Iw * 10.Cubed() * 10.RestTo(6); /// TODO: The multipliers seems to be incorrect and confusing. Check
            wsCalcs.Cells[sRow + 32, sColumn].Value = section.Alpha;

            ///worksheet.Cells[sRow + 33, sCol].Value = section.K;
            ///worksheet.Cells[sRow + 34, sCol].Value = section.K1;
            wsCalcs.Cells[sRow + 35, sColumn].Value = section.Cy;
            wsCalcs.Cells[sRow + 36, sColumn].Value = section.Ey;
            wsCalcs.Cells[sRow + 37, sColumn].Value = section.Cz;
            wsCalcs.Cells[sRow + 38, sColumn].Value = section.Ez;
            wsCalcs.Cells[sRow + 39, sColumn].Value = section.Iu;
            wsCalcs.Cells[sRow + 40, sColumn].Value = section.Iv;
            wsCalcs.Cells[sRow + 41, sColumn].Value = section.Ru;
            wsCalcs.Cells[sRow + 42, sColumn].Value = section.Rv;
        }
    }
}

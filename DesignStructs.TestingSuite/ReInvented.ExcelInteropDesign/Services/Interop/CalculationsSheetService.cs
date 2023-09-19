using System;

using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Shared;

///using Excel = Microsoft.Office.Interop.Excel;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class CalculationsSheetService
    {
        #region Public Methods

        public static void FillMaterialProperties(Worksheet wsCalcs, MaterialTable table, MaterialGrade grade)
        {
            wsCalcs.Range[RangeNames.MaterialSpecification].Value2 = table.Name;
            wsCalcs.Range[RangeNames.MaterialGrade].Value2 = grade.Designation;
            wsCalcs.Range[RangeNames.Fy].Value2 = grade.Fy;
            wsCalcs.Range[RangeNames.Fu].Value2 = grade.Fu;
        }

        public static void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters parameters)
        {
            if (parameters is HSectionAxialStrengthParameters hParameters)
            {
                FillHSectionAxialStrengthParameters(wsCalcs, hParameters);
            }
            else if (parameters is CSectionAxialStrengthParameters cParameters)
            {
                FillCSectionAxialStrengthParameters(wsCalcs, cParameters);
            }
            else if (parameters is LSectionAxialStrengthParameters lParameters)
            {
                FillLSectionAxialStrengthParameters(wsCalcs, lParameters);
            }
            else if (parameters is BoxOrOSectionAxialStrengthParameters boxOrOParameters)
            {
                FillBoxOrOSectionAxialStrengthParameters(wsCalcs, boxOrOParameters);
            }
        }

        public static void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters parameters)
        {
            if (parameters is HSectionShearStrengthParameters hParameters)
            {
                FillHSectionShearStrengthParameters(wsCalcs, hParameters);
            }
            else if (parameters is CSectionShearStrengthParameters cParameters)
            {
                FillCSectionShearStrengthParameters(wsCalcs, cParameters);
            }
            else if (parameters is LSectionShearStrengthParameters lParameters)
            {
                FillLSectionShearStrengthParameters(wsCalcs, lParameters);
            }
            else if (parameters is OSectionShearStrengthParameters oParameters)
            {
                FillOSectionShearStrengthParameters(wsCalcs, oParameters);
            }
            else if (parameters is BoxSectionShearStrengthParameters boxParameters)
            {
                FillBoxSectionShearStrengthParameters(wsCalcs, boxParameters);
            }
        }


        public static void FillMethodOfDesign(Worksheet wsCalcs, DesignMethod designMethod)
        {
            wsCalcs.Range[RangeNames.DesignMethod].Value2 = designMethod.GetDescription();
        }

        public static void FillSectionProperties(Worksheet wsCalcs, IRolledSection section)
        {
            Range rngSectionProperties = wsCalcs.Range[RangeNames.SectionProperties];
            int sRow = rngSectionProperties.Row;
            int sColumn = rngSectionProperties.Column;

            rngSectionProperties.ClearContents();

            FillCommonProperties(wsCalcs, section, sRow, sColumn);

            if (section is RolledSectionHShape || section is RolledSectionCShape)
            {
                FillHAndCSpecificProperties(wsCalcs, section as IRolledSectionHAndC, sRow, sColumn);
            }
            else if (section is RolledSectionLShape lSection)
            {
                throw new NotImplementedException($"Filling properties for {typeof(RolledSectionLShape)} is not implemented yet. Check back later.");
            }
            else if (section is RolledSectionBoxShape boxSection)
            {
                FillBoxSpecificProperties(wsCalcs, boxSection, sRow, sColumn);
            }
            else
            {
                FillOSpecificProperties(wsCalcs, section as RolledSectionOShape, sRow, sColumn);
            }
        }

        #endregion

        #region Private Helpers - Fill Section Properties

        private static void FillCommonProperties(Worksheet wsCalcs, IRolledSection section, int sRow, int sColumn)
        {
            wsCalcs.Cells[sRow + 00, sColumn].Value = section.Designation;
            wsCalcs.Cells[sRow + 01, sColumn].Value = section.Mass;
            wsCalcs.Cells[sRow + 02, sColumn].Value = section.MassFps;

            wsCalcs.Cells[sRow + 09, sColumn].Value = section.A * 10.Squared();
            wsCalcs.Cells[sRow + 15, sColumn].Value = section.ALO;
            wsCalcs.Cells[sRow + 17, sColumn].Value = section.AGO;

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
            wsCalcs.Cells[sRow + 30, sColumn].Value = section.It * 10.RestTo(4);
            wsCalcs.Cells[sRow + 31, sColumn].Value = section.Iw * 10.Cubed() * 10.RestTo(6); /// TODO: The multipliers seems to be incorrect and confusing. Check

            wsCalcs.Cells[sRow + 35, sColumn].Value = section.Cy;
            wsCalcs.Cells[sRow + 36, sColumn].Value = section.Ey;
            wsCalcs.Cells[sRow + 37, sColumn].Value = section.Cz;
            wsCalcs.Cells[sRow + 38, sColumn].Value = section.Ez;
            wsCalcs.Cells[sRow + 39, sColumn].Value = section.Iu;
            wsCalcs.Cells[sRow + 40, sColumn].Value = section.Iv;
            wsCalcs.Cells[sRow + 41, sColumn].Value = section.Ru;
            wsCalcs.Cells[sRow + 42, sColumn].Value = section.Rv;
        }

        private static void FillHAndCSpecificProperties(Worksheet wsCalcs, IRolledSectionHAndC section, int sRow, int sColumn)
        {
            wsCalcs.Cells[sRow + 03, sColumn].Value = section.H;
            wsCalcs.Cells[sRow + 04, sColumn].Value = section.Bf;
            wsCalcs.Cells[sRow + 05, sColumn].Value = section.Tw;
            wsCalcs.Cells[sRow + 06, sColumn].Value = section.Tf;

            wsCalcs.Cells[sRow + 07, sColumn].Value = section.R1;
            wsCalcs.Cells[sRow + 08, sColumn].Value = section.R2;
            wsCalcs.Cells[sRow + 10, sColumn].Value = section.Hi;
            wsCalcs.Cells[sRow + 11, sColumn].Value = section.D;
            wsCalcs.Cells[sRow + 29, sColumn].Value = section.Ss;
            wsCalcs.Cells[sRow + 32, sColumn].Value = section.Alpha;
        }

        private static void FillBoxSpecificProperties(Worksheet wsCalcs, RolledSectionBoxShape section, int sRow, int sColumn)
        {
            wsCalcs.Cells[sRow + 04, sColumn].Value = section.B;
            wsCalcs.Cells[sRow + 06, sColumn].Value = section.Tw;

            wsCalcs.Cells[sRow + 16, sColumn].Value = section.ALI;
            wsCalcs.Cells[sRow + 18, sColumn].Value = section.AGI;
        }

        private static void FillOSpecificProperties(Worksheet wsCalcs, RolledSectionOShape section, int sRow, int sColumn)
        {
            wsCalcs.Cells[sRow + 00, sColumn].Value = section.Designation;
            wsCalcs.Cells[sRow + 01, sColumn].Value = section.Mass;
            wsCalcs.Cells[sRow + 02, sColumn].Value = section.MassFps;

            wsCalcs.Cells[sRow + 03, sColumn].Value = section.OD;
            wsCalcs.Cells[sRow + 04, sColumn].Value = section.ID;
            wsCalcs.Cells[sRow + 05, sColumn].Value = section.Tw;

            wsCalcs.Cells[sRow + 16, sColumn].Value = section.ALI;
            wsCalcs.Cells[sRow + 18, sColumn].Value = section.AGI;
        }

        #endregion

        #region Private Helpers - Fill Axial Strength Parameters

        private static void FillHSectionAxialStrengthParameters(Worksheet wsCalcs, HSectionAxialStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.Anet].Value2 = parameters.Anet;
            wsCalcs.Range[RangeNames.Lus].Value2 = parameters.Lus;
            wsCalcs.Range[RangeNames.Ltub].Value2 = parameters.Ltub;
        }

        private static void FillCSectionAxialStrengthParameters(Worksheet wsCalcs, CSectionAxialStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.Anet].Value2 = parameters.Anet;
            wsCalcs.Range[RangeNames.Lus].Value2 = parameters.Lus;
            wsCalcs.Range[RangeNames.Ltub].Value2 = parameters.Ltub;

            wsCalcs.Range[RangeNames.GussetsConnectedTo].Value2 = parameters.GussetConnectedTo.GetDescription();
            wsCalcs.Range[RangeNames.Tg].Value2 = parameters.Tg;
            wsCalcs.Range[RangeNames.Lcon].Value2 = parameters.Lcon;
        }

        private static void FillLSectionAxialStrengthParameters(Worksheet wsCalcs, LSectionAxialStrengthParameters parameters)
        {
            throw new NotImplementedException($"Filling {nameof(LSectionAxialStrengthParameters)} is not implemented yet.");
        }

        private static void FillBoxOrOSectionAxialStrengthParameters(Worksheet wsCalcs, BoxOrOSectionAxialStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.Lus].Value2 = parameters.Lus;
            wsCalcs.Range[RangeNames.Lcon].Value2 = parameters.Lcon;
            wsCalcs.Range[RangeNames.Tg].Value2 = parameters.Tg;
        }

        #endregion

        #region Private Helpers - Fill Shear Strength Parameters

        private static void FillHSectionShearStrengthParameters(Worksheet wsCalcs, HSectionShearStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.HasStiffeners].Value2 = parameters.StiffenersConfiguration != WebTransverseStiffenersConfiguration.None ? "Yes" : "No";
            wsCalcs.Range[RangeNames.StiffenersConfiguration].Value2 = parameters.StiffenersConfiguration.GetDescription();
            wsCalcs.Range[RangeNames.Ts].Value2 = parameters.Ts;
            wsCalcs.Range[RangeNames.Spacing].Value2 = parameters.Spacing;
        }

        private static void FillCSectionShearStrengthParameters(Worksheet wsCalcs, CSectionShearStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.HasStiffeners].Value2 = parameters.StiffenersConfiguration != WebTransverseStiffenersConfiguration.None ? "Yes" : "No";
            wsCalcs.Range[RangeNames.Ts].Value2 = parameters.Ts;
            wsCalcs.Range[RangeNames.Spacing].Value2 = parameters.Spacing;
        }

        private static void FillLSectionShearStrengthParameters(Worksheet wsCalcs, LSectionShearStrengthParameters parameters)
        {
            throw new NotImplementedException($"Filling {nameof(LSectionShearStrengthParameters)} is not implemented yet.");
        }

        private static void FillOSectionShearStrengthParameters(Worksheet wsCalcs, OSectionShearStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.HssForm].Value2 = parameters.HssForm.GetDescription();
        }

        private static void FillBoxSectionShearStrengthParameters(Worksheet wsCalcs, BoxSectionShearStrengthParameters parameters)
        {
            wsCalcs.Range[RangeNames.HssForm].Value2 = parameters.HssForm.GetDescription();
            wsCalcs.Range[RangeNames.GussetConfiguration].Value2 = parameters.GussetConfiguration.GetDescription();
            wsCalcs.Range[RangeNames.GussetsConnectedTo].Value2 = parameters.GussetConnectedTo.GetDescription();
        }

        #endregion
    }
}

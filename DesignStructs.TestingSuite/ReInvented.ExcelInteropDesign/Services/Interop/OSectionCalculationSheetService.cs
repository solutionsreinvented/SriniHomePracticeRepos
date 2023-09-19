
using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Shared;

namespace ReInvented.ExcelInteropDesign.Services
{
    public sealed class OSectionCalculationSheetService : CalculationSheetService, ICalculationSheetService
    {
        #region Private Fields

        private static ICalculationSheetService _instance;

        #endregion

        #region Private Constructor

        private OSectionCalculationSheetService()
        {

        }

        #endregion

        #region Instance Provider

        public static ICalculationSheetService Instance => _instance ?? (_instance = new OSectionCalculationSheetService());

        #endregion

        #region Abstract Methods Implementation

        public override void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters)
        {
            FillBoxOrOAxialStrengthParameters(wsCalcs, axialStrengthParameters);
        }

        public override void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters)
        {
            OSectionShearStrengthParameters parameters = shearStrengthParameters as OSectionShearStrengthParameters;

            wsCalcs.Range[RangeNames.HssForm].Value2 = parameters.HssForm.GetDescription();
        }

        public override void FillSectionProperties(Worksheet wsCalcs, IRolledSection section)
        {
            Range rngSectionProperties = wsCalcs.Range[RangeNames.SectionProperties];
            int sRow = rngSectionProperties.Row;
            int sColumn = rngSectionProperties.Column;

            rngSectionProperties.ClearContents();

            FillSectionCommonProperties(wsCalcs, section, sRow, sColumn);

            RolledSectionOShape oSection = section as RolledSectionOShape;

            wsCalcs.Cells[sRow + 00, sColumn].Value = oSection.Designation;
            wsCalcs.Cells[sRow + 01, sColumn].Value = oSection.Mass;
            wsCalcs.Cells[sRow + 02, sColumn].Value = oSection.MassFps;
            wsCalcs.Cells[sRow + 03, sColumn].Value = oSection.OD;
            wsCalcs.Cells[sRow + 04, sColumn].Value = oSection.ID;
            wsCalcs.Cells[sRow + 05, sColumn].Value = oSection.Tw;
            wsCalcs.Cells[sRow + 16, sColumn].Value = oSection.ALI;
            wsCalcs.Cells[sRow + 18, sColumn].Value = oSection.AGI;
        } 

        #endregion
    }
}

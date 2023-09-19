
using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Shared;

namespace ReInvented.ExcelInteropDesign.Services
{

    public sealed class BoxSectionCalculationSheetService : CalculationSheetService, ICalculationSheetService
    {
        #region Private Fields

        private static ICalculationSheetService _instance;

        #endregion

        #region Private Constructor

        private BoxSectionCalculationSheetService()
        {

        }

        #endregion

        #region Instance Provider

        public static ICalculationSheetService Instance => _instance ?? (_instance = new BoxSectionCalculationSheetService());

        #endregion

        #region Abstract Methods Implementation

        public override void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters)
        {
            FillBoxOrOAxialStrengthParameters(wsCalcs, axialStrengthParameters);
        }

        public override void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters)
        {
            BoxSectionShearStrengthParameters parameters = shearStrengthParameters as BoxSectionShearStrengthParameters;

            wsCalcs.Range[RangeNames.HssForm].Value2 = parameters.HssForm.GetDescription();
            wsCalcs.Range[RangeNames.GussetConfiguration].Value2 = parameters.GussetConfiguration.GetDescription();
            wsCalcs.Range[RangeNames.GussetsConnectedTo].Value2 = parameters.GussetConnectedTo.GetDescription();
        }

        public override void FillSectionProperties(Worksheet wsCalcs, IRolledSection section)
        {
            Range rngSectionProperties = wsCalcs.Range[RangeNames.SectionProperties];
            int sRow = rngSectionProperties.Row;
            int sColumn = rngSectionProperties.Column;

            rngSectionProperties.ClearContents();

            FillSectionCommonProperties(wsCalcs, section, sRow, sColumn);

            RolledSectionBoxShape boxSection = section as RolledSectionBoxShape;

            wsCalcs.Cells[sRow + 04, sColumn].Value = boxSection.B;
            wsCalcs.Cells[sRow + 06, sColumn].Value = boxSection.Tw;

            wsCalcs.Cells[sRow + 16, sColumn].Value = boxSection.ALI;
            wsCalcs.Cells[sRow + 18, sColumn].Value = boxSection.AGI;
        }

        #endregion
    }
}

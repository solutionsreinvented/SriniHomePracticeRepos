
using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.Design.ExcelInterop.Base;
using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Domain.Design.ExcelInterop.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Shared;

namespace ReInvented.Domain.Design.ExcelInterop.Services
{
    public sealed class HSectionCalculationSheetService : CalculationSheetService, ICalculationSheetService
    {
        #region Private Fields

        private static ICalculationSheetService _instance;

        #endregion

        #region Private Constructor

        private HSectionCalculationSheetService()
        {

        }

        #endregion

        #region Instance Provider

        public static ICalculationSheetService Instance => _instance ?? (_instance = new HSectionCalculationSheetService());

        #endregion

        #region Abstract Methods Implementation

        public override void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters)
        {
            HSectionAxialStrengthParameters parameters = axialStrengthParameters as HSectionAxialStrengthParameters;

            wsCalcs.Range[RangeNames.Anet].Value2 = parameters.Anet;
            wsCalcs.Range[RangeNames.Lus].Value2 = parameters.Lus;
            wsCalcs.Range[RangeNames.Ltub].Value2 = parameters.Ltub;
        }

        public override void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters)
        {
            HSectionShearStrengthParameters parameters = shearStrengthParameters as HSectionShearStrengthParameters;

            wsCalcs.Range[RangeNames.HasStiffeners].Value2 = parameters.StiffenersConfiguration != WebTransverseStiffenersConfiguration.None ? "Yes" : "No";
            wsCalcs.Range[RangeNames.StiffenersConfiguration].Value2 = parameters.StiffenersConfiguration.GetDescription();
            wsCalcs.Range[RangeNames.Ts].Value2 = parameters.Ts;
            wsCalcs.Range[RangeNames.Spacing].Value2 = parameters.Spacing;
        }

        public override void FillSectionProperties(Worksheet wsCalcs, IRolledSection section)
        {
            Range rngSectionProperties = wsCalcs.Range[RangeNames.SectionProperties];
            int sRow = rngSectionProperties.Row;
            int sColumn = rngSectionProperties.Column;

            rngSectionProperties.ClearContents();

            FillSectionCommonProperties(wsCalcs, section, sRow, sColumn);

            IRolledSectionHAndC rolledHSection = section as IRolledSectionHAndC;

            FillHAndCSpecificProperties(wsCalcs, rolledHSection, sRow, sColumn);
        }

        #endregion
    }
}

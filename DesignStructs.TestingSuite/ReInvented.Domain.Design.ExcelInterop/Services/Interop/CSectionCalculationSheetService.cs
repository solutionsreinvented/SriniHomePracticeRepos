
using Microsoft.Office.Interop.Excel;

using ReInvented.Domain.Design.ExcelInterop.Base;
using ReInvented.Domain.Design.ExcelInterop.Enums;
using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Domain.Design.ExcelInterop.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Shared;

namespace ReInvented.Domain.Design.ExcelInterop.Services
{
    public sealed class CSectionCalculationSheetService : CalculationSheetService, ICalculationSheetService
    {
        #region Private Fields

        private static ICalculationSheetService _instance;

        #endregion

        #region Private Constructor

        private CSectionCalculationSheetService()
        {

        }

        #endregion

        #region Instance Provider

        public static ICalculationSheetService Instance => _instance ?? (_instance = new CSectionCalculationSheetService());

        #endregion

        #region Abstract Methods Implementation

        public override void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters)
        {
            CSectionAxialStrengthParameters parameters = axialStrengthParameters as CSectionAxialStrengthParameters;

            wsCalcs.Range[RangeNames.Anet].Value2 = parameters.Anet;
            wsCalcs.Range[RangeNames.Lus].Value2 = parameters.Lus;
            wsCalcs.Range[RangeNames.Ltub].Value2 = parameters.Ltub;

            wsCalcs.Range[RangeNames.GussetsConnectedTo].Value2 = parameters.GussetConnectedTo.GetDescription();
            wsCalcs.Range[RangeNames.Tg].Value2 = parameters.Tg;
            wsCalcs.Range[RangeNames.Lcon].Value2 = parameters.Lcon;
        }

        public override void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters)
        {
            CSectionShearStrengthParameters parameters = shearStrengthParameters as CSectionShearStrengthParameters;

            wsCalcs.Range[RangeNames.HasStiffeners].Value2 = parameters.StiffenersConfiguration != WebTransverseStiffenersConfiguration.None ? "Yes" : "No";
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

            IRolledSectionHAndC rolledCSection = section as IRolledSectionHAndC;

            FillHAndCSpecificProperties(wsCalcs, rolledCSection, sRow, sColumn);
        } 

        #endregion
    }
}

using System;

using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Base;
using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.ExcelInteropDesign.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Services
{
    public sealed class LSectionCalculationSheetService : CalculationSheetService, ICalculationSheetService
    {
        #region Private Fields

        private static ICalculationSheetService _instance;

        #endregion

        #region Private Constructor

        private LSectionCalculationSheetService()
        {

        }

        #endregion

        #region Instance Provider

        public static ICalculationSheetService Instance => _instance ?? (_instance = new LSectionCalculationSheetService());

        #endregion

        #region Abstract Methods Implementation

        public override void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters)
        {
            LSectionAxialStrengthParameters parameters = axialStrengthParameters as LSectionAxialStrengthParameters;

            throw new NotImplementedException($"Filling {nameof(LSectionAxialStrengthParameters)} is not implemented yet.");
        }

        public override void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters)
        {
            LSectionShearStrengthParameters parameters = shearStrengthParameters as LSectionShearStrengthParameters;

            throw new NotImplementedException($"Filling {nameof(LSectionShearStrengthParameters)} is not implemented yet.");
        }

        public override void FillSectionProperties(Worksheet wsCalcs, IRolledSection section)
        {
            throw new NotImplementedException($"Filling properties for {typeof(RolledSectionLShape)} is not implemented yet. Check back later.");
        } 

        #endregion
    }
}

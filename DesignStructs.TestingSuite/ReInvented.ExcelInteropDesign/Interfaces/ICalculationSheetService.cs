using Microsoft.Office.Interop.Excel;

using ReInvented.ExcelInteropDesign.Enums;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.ExcelInteropDesign.Interfaces
{
    public interface ICalculationSheetService
    {
        void FillMaterialProperties(Worksheet wsCalcs, MaterialTable table, MaterialGrade grade);
        void FillMethodOfDesign(Worksheet wsCalcs, DesignMethod designMethod);
        void FillAxialStrengthParameters(Worksheet wsCalcs, IAxialStrengthParameters axialStrengthParameters);
        void FillShearStrengthParameters(Worksheet wsCalcs, IShearStrengthParameters shearStrengthParameters);
        void FillSectionProperties(Worksheet wsCalcs, IRolledSection section);
    }
}

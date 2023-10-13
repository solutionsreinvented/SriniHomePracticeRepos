using ReInvented.Domain.Design.ExcelInterop.Interfaces;
using ReInvented.Domain.Design.ExcelInterop.Services;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace ReInvented.Domain.Design.ExcelInterop.Factories
{
    public class CalculationSheetServiceFactory
    {
        public static ICalculationSheetService Get<TSection>() where TSection : IRolledSection
        {
            ICalculationSheetService calculationSheetService;

            if (typeof(TSection) == typeof(RolledSectionHShape))
            {
                calculationSheetService = HSectionCalculationSheetService.Instance;
            }
            else if (typeof(TSection) == typeof(RolledSectionCShape))
            {
                calculationSheetService = CSectionCalculationSheetService.Instance;
            }
            else if (typeof(TSection) == typeof(RolledSectionLShape))
            {
                calculationSheetService = LSectionCalculationSheetService.Instance;
            }
            else if (typeof(TSection) == typeof(RolledSectionOShape))
            {
                calculationSheetService = OSectionCalculationSheetService.Instance;
            }
            else
            {
                calculationSheetService = BoxSectionCalculationSheetService.Instance;
            }

            return calculationSheetService;
        }
    }
}

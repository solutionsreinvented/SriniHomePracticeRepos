using SPro2023ConsoleApp.Base;
using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Models
{
    public class PlateMTOTableRow : MTOTableRow, IMTOTableRow
    {
        public double Thickness { get; set; }

        public double TotalPlanArea { get; set; }
    }
}

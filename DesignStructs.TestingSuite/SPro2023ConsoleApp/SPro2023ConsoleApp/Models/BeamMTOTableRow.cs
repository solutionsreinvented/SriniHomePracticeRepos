using SPro2023ConsoleApp.Base;
using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Models
{
    public class BeamMTOTableRow : MTOTableRow, IMTOTableRow
    {
        public string PropertyName { get; set; }

        public double TotalLength { get; set; }

    }
}


using ReInvented.Sections.Domain.Models;

using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Base
{
    public abstract class MTOTableRow : IMTOTableRow
    {
        public string Description { get; set; }

        public double TotalWeight { get; set; }

        public MaterialGrade MaterialGrade { get; set; }
    }
}

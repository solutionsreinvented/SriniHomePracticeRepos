using System.Collections.Generic;

using ReInvented.Sections.Domain.Models;

namespace SPro2023ConsoleApp.Interfaces
{
    public interface IMTOTableRow
    {
        string Description { get; set; }

        MaterialGrade MaterialGrade { get; set; }

        double TotalWeight { get; set; }
    }

}

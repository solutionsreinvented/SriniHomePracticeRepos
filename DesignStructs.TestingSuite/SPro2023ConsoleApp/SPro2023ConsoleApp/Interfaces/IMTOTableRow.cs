using System.Collections.Generic;

using ReInvented.Sections.Domain.Models;

namespace SPro2023ConsoleApp.Interfaces
{
    public interface IMTOTableRow
    {
        List<int> EntitiesIds { get; set; }

        int ObjectsCount { get; }

        string Description { get; set; }

        MaterialGrade MaterialGrade { get; set; }

        double TotalWeight { get; set; }
    }
}

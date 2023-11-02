using System.Collections.Generic;

using ReInvented.Sections.Domain.Models;

using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Base
{
    public abstract class MTOTableRow : IMTOTableRow
    {
        public List<int> EntitiesIds { get; set; }

        public int ObjectsCount => EntitiesIds.Count;

        public string Description { get; set; }

        public MaterialGrade MaterialGrade { get; set; }

        public double TotalWeight { get; set; }
    }
}

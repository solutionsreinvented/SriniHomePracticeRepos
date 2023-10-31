using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Base
{
    public abstract class MTOTableRow : IMTOTableRow
    {
        public int ObjectsCount { get; set; }

        public double TotalWeight { get; set; }

        public string MaterialGrade { get; set; }
    }
}

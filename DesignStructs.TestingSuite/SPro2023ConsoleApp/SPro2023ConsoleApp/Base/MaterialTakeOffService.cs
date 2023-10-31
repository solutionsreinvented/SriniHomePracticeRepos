using ReInvented.StaadPro.Interactivity.Models;

using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Base
{
    public abstract class MaterialTakeOffService : IMaterialTakeOffService
    {
        public abstract void GenerateMTO(StaadModelWrapper wrapper);
    }
}


using ReInvented.Domain.ProjectSetup.Models;
using ReInvented.Domain.TankAndSupportStructure.Common.Interfaces;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IProject
    {
        IThickenerInput Input { get; }

        ISettings Settings { get; }
    }
}

using ReInvented.Domain.ProjectSetup.Interfaces;
using ReInvented.Domain.Tass.Common.Interfaces;

namespace SPro2023ConsoleApp
{
    public static class ProjectExtensions
    {
        public static bool IsValid(this IProject project)
        {
            IInput input = project.Input;
            ISettings settings = project.Settings;

            bool validationResult = false;







            return false;
        }
    }
}

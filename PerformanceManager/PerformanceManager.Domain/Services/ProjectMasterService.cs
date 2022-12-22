using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Models;

namespace PerformanceManager.Domain.Services
{
    public class ProjectMasterService
    {
        public static ProjectMaster Retrieve()
        {
            /// TODO: In real-world scenario this needs to generated from a json file.
            
            ProjectMaster projectMaster = new();

            IProject preOrder1 = new PreOrder("GS2212129", "Liberia");
            IProject preOrder2 = new PreOrder("GS2212130", "Rilebia");
            IProject preOrder3 = new PreOrder("GS2212131", "Bileria");

            IProject order1 = new Order("22-3849", "25m Thickener");
            IProject order2 = new Order("22-3877", "48m Thickener");
            IProject order3 = new Order("22-3965", "77m Thickener");

            projectMaster.Projects.Add(preOrder1);
            projectMaster.Projects.Add(preOrder2);
            projectMaster.Projects.Add(preOrder3);
            projectMaster.Projects.Add(order1);
            projectMaster.Projects.Add(order2);
            projectMaster.Projects.Add(order3);

            return projectMaster;
        }
    }
}

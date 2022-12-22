using System.Linq;
using System.Windows;

using Techture.PerformanceManager.Enums;
using Techture.PerformanceManager.Models;

namespace Techture.PerformanceManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            string projectCode = "22-3849";

            ProjectMaster projectMaster = GenerateProjectMaster();


            Project project = projectMaster.Projects.FirstOrDefault(p => p.Code == projectCode);

            project.AddActivity(GenerateActivity(ActivityType.Design));

            base.OnStartup(e);
        }

        private Activity GenerateActivity(ActivityType activityType)
        {
            if (activityType == ActivityType.Design)
            {
                return new DesignActivity(1);
            }
            else
            {
                return new DetailingActivity(1);
            }
        }

        private ProjectMaster GenerateProjectMaster()
        {
            int projectId = 1;

            ProjectMaster master = new ProjectMaster();

            master.Projects.Add(new Order(projectId++) { Code = "22-3849", Name = "Liberia 48m Thickener" });
            master.Projects.Add(new Order(projectId++) { Code = "22-3849", Name = "Liberia 25m Thickener" });
            master.Projects.Add(new PreOrder(projectId++) { Code = "GS2212128", Name = "Anglo American 65m Thickener" });
            master.Projects.Add(new PreOrder(projectId++) { Code = "GS2212129", Name = "Anglo American 77m Thickener" });


            return master;
        }
    }
}

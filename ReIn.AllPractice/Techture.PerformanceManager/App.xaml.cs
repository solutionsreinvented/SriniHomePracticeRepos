using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

using Newtonsoft.Json;

using Techture.PerformanceManager.Enums;
using Techture.PerformanceManager.Models;

namespace Techture.PerformanceManager
{
    public class ActivityMaster
    {
        public List<DomainClass> Domains { get; set; }
    }
    public enum DomainGroup
    {
        Design,
        Detailing
    }
    public class DomainClass
    {
        public DomainGroup Group { get; set; }

        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public string Type { get; set; }

        public List<string> SubCategories { get; set; }
    }


    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ActivityMaster CreateActivityMaster()
        {
            ActivityMaster activityMaster = new ActivityMaster() { Domains = new List<DomainClass>() };
            activityMaster.Domains.Add(new DomainClass()
            {
                Group = DomainGroup.Design,
                Categories = new List<Category>()
                {
                    new Category()
                    {
                        Type = "Non Structural",
                        SubCategories = new List<string>()
                                        { "Preamble", "Foundation Load Data", "Base Plates Design"}
                    },
                    new Category()
                    {
                        Type = "Structural",
                        SubCategories = new List<string>()
                                        { "Staad Modeling", "Loads Application",
                                          "Analysis and Design", "Member Design Calculation Report",
                                          "Connections Design Calculation Report"
                                        }
                    }
                }
            });

            activityMaster.Domains.Add(new DomainClass()
            {
                Group = DomainGroup.Detailing,
                Categories = new List<Category>()
                {
                    new Category()
                    {
                        Type = "Structural",
                        SubCategories = new List<string>()
                                        { "Tekla Modeling", "Design Drawings",
                                          "Fabrication Drawings", "Erection Drawings",
                                          "Corrections and Revisions"
                                        }
                    }
                }
            });

            return activityMaster;
        }

        private void WriteJsonToFile(string contents)
        {
            string path = @"C:\Users\masanams\source\SriniHomePracticeRepos\ReIn.AllPractice\Techture.PerformanceManager\Data\ActivityMaster.json";

            File.WriteAllText(path, contents);
        }

        private ActivityMaster ReadCategories()
        {
            string fileContents = File.ReadAllText(@"C:\Users\masanams\source\SriniHomePracticeRepos\ReIn.AllPractice\Techture.PerformanceManager\Data\ActivityCategories.json");

            return JsonConvert.DeserializeObject<ActivityMaster>(fileContents);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ActivityMaster actMaster = CreateActivityMaster();

            WriteJsonToFile(JsonConvert.SerializeObject(CreateActivityMaster()));

            ActivityMaster categories = ReadCategories();


            string projectCode = "22-3849";

            ProjectMaster projectMaster = GenerateProjectMaster();


            Project project = projectMaster.Projects.FirstOrDefault(p => p.Code == projectCode);

            project.AddActivity(GenerateActivity(ActivityType.Design));

            base.OnStartup(e);
        }

        private Activity GenerateActivity(ActivityType activityType)
        {
            return activityType == ActivityType.Design ? new DesignActivity(1) : (Activity)new DetailingActivity(1);
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

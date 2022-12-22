using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Techture.PerformanceManager.Models
{

    public class ProjectMaster
    {
        public ProjectMaster()
        {
            Projects = new ObservableCollection<Project>();
        }
        public ObservableCollection<Project> Projects { get; set; }

        public void AddProject(Project project)
        {
            if (Projects.Contains(project))
                return;

            Projects.Add(project);
        }

    }

    public class PreOrder : Project
    {
        public PreOrder(int id) : base(id)
        {

        }
    }

    public class Project
    {
        public Project(int id)
        {
            Id = id;
            Activities = new ObservableCollection<Activity>();
        }

        public int Id { get; protected set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public ObservableCollection<Activity> Activities { get; set; }

        public void AddActivity(Activity activity)
        {
            if (Activities.Contains(activity))
                return;

            Activities.Add(activity);
        }
    }

    public class Order : Project
    {
        public Order(int id) : base(id)
        {

        }
    }

    public class DesignActivity : Activity
    {
        public DesignActivity(int id) : base(id)
        {

        }
    }

    public class DetailingActivity : Activity
    {
        public DetailingActivity(int id) : base(id)
        {

        }
    }

    public class Activity
    {
        public Activity(int id)
        {
            Id = id;
        }

        public int Id { get; protected set; }
    }

}

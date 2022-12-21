using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Models
{
    public class Project : PropertyStore
    {

        #region Default Constructor

        public Project() : this(ProjectType.Order)
        {

        }

        #endregion

        #region Parameterized Constructors

        public Project(ProjectType projectType)
        {
            Type = projectType;
        }

        public Project(ProjectType projectType, int projectCode, string name) : this(projectType)
        {
            Code = projectCode;
            Name = name;
        }

        #endregion

        public ProjectType Type { get => Get<ProjectType>(); set => Set(value); }

        public int Code { get => Get<int>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }
    }
}

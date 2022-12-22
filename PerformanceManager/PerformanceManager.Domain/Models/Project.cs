using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Interfaces;
using PerformanceManager.Domain.Stores;

using System.Collections.ObjectModel;

namespace PerformanceManager.Domain.Models
{
    public abstract class Project : PropertyStore, IProject
    {

        #region Parameterized Constructors

        public Project(string code, string name)
        {
            Code = code;
            Name = name;
            Activities = new ObservableCollection<IActivity>();
        }

        #endregion

        public ProjectType Type { get => Get<ProjectType>(); protected set => Set(value); }

        public string Code { get => Get<string>(); protected set => Set(value); }

        public string Name { get => Get<string>(); protected set => Set(value); }

        public ObservableCollection<IActivity> Activities { get; set; }

        public virtual void AddActivity(IActivity activity)
        {
            if (Activities.Contains(activity))
                return;

            Activities.Add(activity);
        }

        public virtual void RemoveActivity(IActivity activity)
        {
            if (!Activities.Contains(activity))
                return;

            _ = Activities.Remove(activity);
        }
    }

    public class Order : Project
    {
        #region Parameterized Constructors

        public Order(string code, string name) : base(code, name)
        {
            Type = ProjectType.Order;
        }

        #endregion
    }

    public class PreOrder : Project
    {
        #region Parameterized Constructors

        public PreOrder(string code, string name) : base(code, name)
        {
            Type = ProjectType.PreOrder;
        }

        #endregion
    }
}

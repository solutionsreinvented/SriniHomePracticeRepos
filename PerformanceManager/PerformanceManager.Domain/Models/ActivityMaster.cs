using PerformanceManager.Domain.Enums;
using PerformanceManager.Domain.Stores;

using System.Collections.Generic;

namespace PerformanceManager.Domain.Models
{
    public class Category : PropertyStore
    {
        public string Name { get => Get<string>(); set => Set(value); }

        public HashSet<string> SubCategories { get => Get<HashSet<string>>(); set => Set(value); }
    }

    public class ActivityDomain : PropertyStore
    {
        public ActivityDomain()
        {

        }

        public Discipline Descipline { get => Get<Discipline>(); set => Set(value); }

        public HashSet<Category> Categories { get; set; }
    }

    public class ActivityMaster : PropertyStore
    {
        public ActivityMaster()
        {

        }

        public HashSet<ActivityDomain> Domains { get => Get<HashSet<ActivityDomain>>(); set => Set(value); }

    }

}

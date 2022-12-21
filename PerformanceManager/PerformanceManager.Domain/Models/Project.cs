using PerformanceManager.Domain.Stores;

namespace PerformanceManager.Domain.Models
{
    public class Project : PropertyStore
    {



        public Project()
        {

        }

        public int Code { get => Get<int>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }
    }
}

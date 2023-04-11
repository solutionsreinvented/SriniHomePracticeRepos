using ActivityTracker.Domain.Stores;

using System.Collections.Generic;

namespace ActivityTracker.Domain.Models
{
    public class Category : PropertyStore
    {
        public string Name { get => Get<string>(); set => Set(value); }

        public HashSet<string> SubCategories { get => Get<HashSet<string>>(); set => Set(value); }
    }
}

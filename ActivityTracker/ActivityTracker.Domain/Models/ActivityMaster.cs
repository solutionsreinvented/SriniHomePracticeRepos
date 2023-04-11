using ActivityTracker.Domain.Stores;

using System.Collections.Generic;

namespace ActivityTracker.Domain.Models
{
    public class ActivityMaster : PropertyStore
    {
        public ActivityMaster()
        {

        }

        public HashSet<ActivityDomain> Domains { get => Get<HashSet<ActivityDomain>>(); set => Set(value); }

        public HashSet<string> Structures { get => Get<HashSet<string>>(); set => Set(value); }

    }
}

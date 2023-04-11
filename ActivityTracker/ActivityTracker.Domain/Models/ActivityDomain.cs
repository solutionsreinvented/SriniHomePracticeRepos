using ActivityTracker.Domain.Enums;
using ActivityTracker.Domain.Stores;

using System.Collections.Generic;

namespace ActivityTracker.Domain.Models
{
    public class ActivityDomain : PropertyStore
    {
        public ActivityDomain()
        {

        }

        public Discipline Descipline { get => Get<Discipline>(); set => Set(value); }

        public HashSet<Category> Categories { get; set; }
    }
}

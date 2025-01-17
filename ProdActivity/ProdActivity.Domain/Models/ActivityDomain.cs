using ProdActivity.Domain.Enums;
using ProdActivity.Domain.Stores;

using System.Collections.Generic;

namespace ProdActivity.Domain.Models
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

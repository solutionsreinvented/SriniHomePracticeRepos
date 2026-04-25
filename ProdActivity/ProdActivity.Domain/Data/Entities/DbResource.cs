using System.Collections.Generic;

namespace ProdActivity.Domain.Data.Entities
{
    public class DbResource
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; } // Engineer, Detailer, LeadEngineer, etc.
        public List<DbActivityResource> ActivityResources { get; set; } = new List<DbActivityResource>();
    }
}

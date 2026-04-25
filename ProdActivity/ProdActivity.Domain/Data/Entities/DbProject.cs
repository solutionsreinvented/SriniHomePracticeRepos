using System;
using System.Collections.Generic;
using ProdActivity.Domain.Enums;

namespace ProdActivity.Domain.Data.Entities
{
    public class DbProject
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public ProjectType Type { get; set; }
        
        public List<DbActivity> Activities { get; set; } = new List<DbActivity>();
    }
}

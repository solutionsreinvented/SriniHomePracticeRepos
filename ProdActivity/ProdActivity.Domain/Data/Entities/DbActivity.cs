using System;
using System.Collections.Generic;
using ProdActivity.Domain.Enums;

namespace ProdActivity.Domain.Data.Entities
{
    public class DbActivity
    {
        public string Id { get; set; }
        public int ProjectId { get; set; }
        public DbProject Project { get; set; }

        public Discipline Discipline { get; set; }
        public string CategoryName { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }

        public DateTime InitiatedOn { get; set; }
        public double AllocatedHours { get; set; }
        public CompletionStatus CurrentStatus { get; set; }
        public DateTime ScheduledCompletion { get; set; }

        public List<DbActivityResource> ActivityResources { get; set; } = new List<DbActivityResource>();
    }
}

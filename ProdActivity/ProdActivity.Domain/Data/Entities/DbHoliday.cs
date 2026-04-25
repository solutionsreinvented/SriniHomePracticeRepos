using System;

namespace ProdActivity.Domain.Data.Entities
{
    public class DbHoliday
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}

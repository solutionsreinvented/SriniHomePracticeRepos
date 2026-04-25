namespace ProdActivity.Domain.Data.Entities
{
    public class DbActivityResource
    {
        public string ActivityId { get; set; }
        public DbActivity Activity { get; set; }

        public int ResourceId { get; set; }
        public DbResource Resource { get; set; }
    }
}

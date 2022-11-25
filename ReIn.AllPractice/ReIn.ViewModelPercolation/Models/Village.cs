using ReInvented.Shared.Stores;

namespace ReIn.ViewModelPercolation.Models
{
    public class Village : PropertyStore
    {
        public Village()
        {

        }

        public int Id { get => Get<int>(); set => Set(value); }

        public string Name { get => Get<string>(); set => Set(value); }
    }
}

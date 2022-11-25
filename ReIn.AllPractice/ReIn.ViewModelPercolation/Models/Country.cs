using ReInvented.Shared.Stores;

namespace ReIn.ViewModelPercolation.Models
{
    public class Country : PropertyStore
    {
        public Country()
        {
            Village = new Village();
            Family = new Family();
        }

        public Village Village { get => Get<Village>(); set => Set(value); }

        public Family Family { get => Get<Family>(); set => Set(value); }

    }
}

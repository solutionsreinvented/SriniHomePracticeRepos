using ReInvented.Shared.Stores;

namespace ReIn.ViewModelPercolation.Models
{
    public class Family : PropertyStore
    {
        public Family()
        {

        }

        public int Size { get => Get<int>(); set => Set(value); }

        public string Group { get => Get<string>(); set => Set(value); }

    }
}

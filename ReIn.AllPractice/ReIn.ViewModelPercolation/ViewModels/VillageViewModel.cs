using ReIn.ViewModelPercolation.Models;
using ReIn.ViewModelPercolation.Stores;

namespace ReIn.ViewModelPercolation.ViewModels
{
    public class VillageViewModel : BaseViewModel
    {
        public VillageViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Village = new Village();
        }

        public Village Village { get => Get<Village>(); set => Set(value); }
    }
}

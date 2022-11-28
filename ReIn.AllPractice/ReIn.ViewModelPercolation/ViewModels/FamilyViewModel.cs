using ReIn.ViewModelPercolation.Models;
using ReIn.ViewModelPercolation.Stores;

namespace ReIn.ViewModelPercolation.ViewModels
{
    public class FamilyViewModel : BaseViewModel
    {
        public FamilyViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Family = new Family();
        }

        public Family Family { get => Get<Family>(); set => Set(value); }
    }
}

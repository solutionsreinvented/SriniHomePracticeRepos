using ActivityTracker.Domain.Repositories;
using ActivityTracker.UI.Stores;

namespace ActivityTracker.UI.Base
{
    public abstract class ManageUserViewModel : ViewModelBase
    {
        #region Private Fields
        private readonly UserRepository _usersRepository = new();
        #endregion

        #region Parameterized Constructor
        public ManageUserViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Initialize();
        }
        #endregion

        protected abstract void Initialize();
    }
}

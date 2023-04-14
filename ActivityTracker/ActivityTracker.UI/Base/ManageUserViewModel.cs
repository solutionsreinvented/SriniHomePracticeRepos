using ActivityTracker.Domain.Interfaces;
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

        #region Public Properties
        public IUser User { get => Get<IUser>(); protected set => Set(value); }

        public bool IsLoggedIn { get => Get<bool>(); protected set => Set(value); }
        #endregion

        #region Readonly Properties
        public bool UserExists => User != null;
        #endregion

        #region Private Helpers
        protected IUser GetUser(string userId)
        {
            _ = int.TryParse(userId, out int result);

            return _usersRepository.GetById(result);
        }
        #endregion

        #region Abstract Methods
        protected abstract void Initialize();
        #endregion
    }
}

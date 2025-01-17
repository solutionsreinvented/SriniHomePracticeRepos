
using ProdActivity.Domain.Stores;
using ProdActivity.UI.Stores;

using ReInvented.Shared.Interfaces;

namespace ProdActivity.UI.Base
{
    public abstract class ViewModelBase : PropertyStore
    {
        #region Private Fields
        private protected readonly NavigationStore _navigationStore;
        private protected readonly IDialogService _dialogService;
        #endregion

        #region Parameterized Constructor
        public ViewModelBase(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _dialogService = _navigationStore.DialogService;
        }
        #endregion
    }
}

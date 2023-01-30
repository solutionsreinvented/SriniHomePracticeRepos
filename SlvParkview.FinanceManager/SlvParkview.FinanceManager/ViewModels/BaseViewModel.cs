using ReInvented.Shared.Stores;

using SlvParkview.FinanceManager.Models;

namespace SlvParkview.FinanceManager.ViewModels
{
    public class BaseViewModel : PropertyStore
    {
        public Block Block { get => Get<Block>(); set { Set(value); ShowContent = value != null; } }

        public bool ShowContent { get => Get<bool>(); set => Set(value); }

    }
}

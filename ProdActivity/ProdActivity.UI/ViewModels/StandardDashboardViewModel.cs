using ProdActivity.UI.Base;
using ProdActivity.UI.Stores;

namespace ProdActivity.UI.ViewModels
{
    public class StandardDashboardViewModel : DashboardViewModel
    {
        #region Parameterized Constructor

        public StandardDashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            InitializeProperties();
        }

        #endregion

        #region Public Properties

        public string UserPassword { get => Get<string>(); set => Set(value); }

        public string TestTextBoxValue { get => Get<string>(); set => Set(value); }


        //public PreOrder SelectedPreOrder { get => Get<PreOrder>(); set => Set(value); }

        //public Order SelectedOrder { get => Get<Order>(); set => Set(value); } 

        #endregion

        #region Private Methods

        protected override void InitializeProperties()
        {
            Title = "Standard User's Dashboard";
            UserIsAdmin = false;

            base.InitializeProperties();
        }

        #endregion
    }
}

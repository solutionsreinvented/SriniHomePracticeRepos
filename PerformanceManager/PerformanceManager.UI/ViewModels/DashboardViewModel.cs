using PerformanceManager.Domain.Interfaces;
using PerformanceManager.UI.Stores;

using System.Collections.Generic;
using System.Linq;

namespace PerformanceManager.UI.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public DashboardViewModel(NavigationStore navigationStore) : base(navigationStore)
        {
            Title = "I am the dashboard!";
        }

        public string Title { get => Get<string>(); set => Set(value); }

        public IResource Resource { get => Get<IResource>(); set => Set(value); }

        public string UserPassword { get => Get<string>(); set => Set(value); }

        public string TestTextBoxValue { get => Get<string>(); set => Set(value); }
    }
}

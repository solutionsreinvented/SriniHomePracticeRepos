using AllCorePracticeApp.Enums;
using ReInvented.Shared.Stores;

namespace AllCorePracticeApp.ViewModels
{
    internal abstract class BaseViewModel : PropertyStore
    {
        public string Header { get; set; }

        public DesignCode DesignCode { get; set; }

    }
}

using System.Collections.ObjectModel;

using ReInvented.Sections.Domain.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.Domain.Design.ExcelInterop.Models
{
    public class SectionsPreference : ErrorsEnabledPropertyStore
    {
        public SectionsPreference()
        {
            Classifications = new ObservableCollection<Classification>();
        }

        public string Key { get => Get<string>(); set => Set(value); }

        public ObservableCollection<Classification> Classifications { get => Get<ObservableCollection<Classification>>(); set => Set(value); }

    }
}

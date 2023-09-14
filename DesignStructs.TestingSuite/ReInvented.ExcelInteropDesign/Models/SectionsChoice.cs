using System.Collections.ObjectModel;

using ReInvented.Sections.Domain.Models;
using ReInvented.Shared.Stores;

namespace ReInvented.ExcelInteropDesign.Models
{
    public class SectionsChoice : ErrorsEnabledPropertyStore
    {
        public SectionsChoice()
        {
            Classifications = new ObservableCollection<Classification>();
        }

        public string Key { get => Get<string>(); set => Set(value); }

        public ObservableCollection<Classification> Classifications { get => Get<ObservableCollection<Classification>>(); set => Set(value); }

    }
}

using System.Linq;

using ReInvented.Domain.EarthquakeLoading.Models;
using ReInvented.Domain.EarthquakeLoading.Services;
using ReInvented.Shared.Stores;

namespace ReInvented.UI.ViewModels
{
    public class ResponseSystemSelectionViewModel : ValidatablePropertyStore
    {
        public ResponseSystemSelectionViewModel()
        {

        }

        public ResponseSystemSelectionViewModel(ASCE7Tables dataTables)
        {
            if (dataTables == null)
            {
                dataTables = DataTablesService.GetDeserializedData<ASCE7Tables>($"{typeof(ASCE7Tables).Name}.json");
            }
            DataTables = dataTables;
            SelectedSystemGroup = DataTables?.DesignCoefficientsAndFactors?.SystemGroups?.FirstOrDefault();
            SelectedForceResistingSystem = SelectedSystemGroup?.Systems?.FirstOrDefault();
        }

        public ASCE7Tables DataTables { get => Get<ASCE7Tables>(); private set => Set(value); }

        public SeismicForceResistingSystemGroup SelectedSystemGroup { get => Get<SeismicForceResistingSystemGroup>(); set => Set(value); }

        public SeismicForceResistingSystem SelectedForceResistingSystem { get => Get<SeismicForceResistingSystem>(); set => Set(value); }

        public string IconPath => @"C:\Users\masanams\OneDrive - TAKRAF\Desktop\spinning-circle.gif";

    }
}

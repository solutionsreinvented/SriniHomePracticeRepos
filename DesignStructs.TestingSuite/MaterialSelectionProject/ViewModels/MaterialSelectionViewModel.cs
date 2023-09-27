using MaterialSelectionProject.Models;

using ReInvented.Shared.Stores;

namespace MaterialSelectionProject.ViewModels
{
    public class MaterialSelectionViewModel : ErrorsEnabledPropertyStore
    {
        public MaterialSelectionViewModel()
        {
            Material = new Material();
        }

        public Material Material { get => Get<Material>(); set => Set(value); }
    }
}

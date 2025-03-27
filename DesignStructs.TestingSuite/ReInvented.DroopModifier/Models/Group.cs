using ReInvented.Shared.Stores;

namespace ReInvented.DroopModifier.Models
{
    public class Group : ValidatablePropertyStore
    {
        public string Name { get => Get<string>(); set => Set(value); }

        public bool IsSelected { get => Get<bool>(); set => Set(value); }
    }
}

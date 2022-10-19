using ReInvented.Shared.Stores;

namespace ReIn.NavPractice.Models
{
    public class GridRow : PropertyStore
    {
        public GridRow()
        {

        }

        public string Description { get => Get<string>(); set => Set(value); }

        public double PCD { get => Get<double>(); set => Set(value); }

        public Section Column { get => Get<Section>(); set => Set(value); }

        public bool HasRadialBracing { get => Get<bool>(); set => Set(value); }

        public Section RadialBracing { get => Get<Section>(); set => Set(value); }

        public bool HasCrossBracing { get => Get<bool>(); set => Set(value); }

        public Section CrossBracing { get => Get<Section>(); set => Set(value); }



    }
}

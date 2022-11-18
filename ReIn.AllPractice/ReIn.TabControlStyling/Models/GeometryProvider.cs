using System.Windows.Media;

using ReIn.TabControlStyling.Enums;

using ReInvented.Shared.Stores;

namespace ReIn.TabControlStyling.Models
{
    public class GeometryProvider : PropertyStore
    {
        public GeometryProvider(int thickness)
        {
            Thickness = thickness;
        }

        public int Thickness { get => Get<int>(); set => Set(value); }

        public GeometryResourceKey Key { get => Get(GeometryResourceKey.BottomLess); set { Set(value); RaisePropertyChanged(nameof(GeometryData)); } }

        public PathGeometry GeometryData => GetGeometryData();

        private PathGeometry GetGeometryData()
        {
            PathGeometry geom;

            switch (Key)
            {
                case GeometryResourceKey.LeftLess:
                    geom = null;
                    break;
                case GeometryResourceKey.TopLess:
                    geom = null;
                    break;
                case GeometryResourceKey.RightLess:
                    geom = null;
                    break;
                case GeometryResourceKey.BottomLess:
                    geom = null;
                    break;
                default:
                    geom = null;
                    break;
            }

            return geom;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace CoreClassLibrary
{
    internal class Tank
    {
        public Tank()
        {
            
        }

        [ObservableProperty]
        private double _diameter;

        [ObservableProperty]
        private double _sideWallDepth;

        [ObservableProperty]
        private double _freeboard;
    }
}

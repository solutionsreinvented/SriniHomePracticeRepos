using ReIn.VectorsPractice.Models;

using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Fakes
{
    public static class FakeBridgeFactory
    {
        public static Bridge Generate()
        {
            return new Bridge()
            {
                Height = 2.8,
                Radius = 25.0,
                Theta = 15,
                Width = 2.0,
                DeckElevation = 10.0,
                Origin = new Node(0.0, 0.0, 0.0)
            };
        }
    }
}

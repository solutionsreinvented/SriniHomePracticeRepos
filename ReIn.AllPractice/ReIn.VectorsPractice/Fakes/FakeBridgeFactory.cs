using System.Collections.Generic;

using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;

using ReInvented.Shared;
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
                Theta = 139.5,
                Width = 2.0,
                DeckElevation = 10.0,
                Origin = new Node(0.0, 0.0, 0.0),
                FrameGrids = new List<IFrameGrid>()
                {
                    //new BridgeFrameGrid() { Spacing = 0.0, Width = 2.0, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 2.2, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 2.4, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 2.6, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.1, Width = 2.8, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 3.0, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 3.1, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 3.2, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.1, Width = 3.3, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 3.4, Height = 2.8 },
                    //new BridgeFrameGrid() { Spacing = 2.3, Width = 3.5, Height = 2.8 }

                    new BridgeFrameGrid() { Spacing = 0.0, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.1, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.1, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 },
                    new BridgeFrameGrid() { Spacing = 2.3, Width = 2.0, Height = 2.8 }

                }
            };

            //int nFrames = 11;
            //double spacing = bridge.Length / (nFrames - 1);

            //bridge.FrameGrids = new List<IFrameGrid>();

            //for (int i = 0; i < nFrames; i++)
            //{
            //    bridge.FrameGrids.Add(new BridgeFrameGrid() { Spacing = spacing, Width = 2.0, Height = 2.8 });
            //}

            //return bridge;

        }
    }
}

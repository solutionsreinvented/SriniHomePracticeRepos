using System;
using System.Collections.Generic;

using ReIn.VectorsPractice.Interfaces;

using ReInvented.Shared;
using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Models
{

    public class Bridge
    {
        public Bridge()
        {
            FrameGrids = new List<IFrameGrid>();
        }

        public List<IFrameGrid> FrameGrids { get; set; }

        public Node Origin { get; set; }

        public double Length => Radius * Math.Cos((Alpha / 2).ToRadians());

        public double Width { get; set; }

        public double Height { get; set; }

        public double Theta { get; set; }

        public double Alpha => 2 * Math.Asin(Width / 2 / Radius).ToDegrees();

        public double Radius { get; set; }

        public double DeckElevation { get; set; }

    }
}

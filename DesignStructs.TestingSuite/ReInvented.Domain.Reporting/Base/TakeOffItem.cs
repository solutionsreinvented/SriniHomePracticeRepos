﻿namespace ReInvented.Domain.Reporting.Models.Base
{
    public abstract class TakeOffItem
    {
        public int SlNo { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }
    }
}
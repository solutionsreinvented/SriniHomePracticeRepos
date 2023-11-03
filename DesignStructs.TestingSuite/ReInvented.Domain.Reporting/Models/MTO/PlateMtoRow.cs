using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Reporting.Models
{
    public class PlateMtoRow : MtoTableRow, IMtoTableRow
    {
        public PlateMtoRow()
        {
            Plates = new HashSet<Plate>();
        }

        public HashSet<Plate> Plates { get; private set; }

        public double Thickness { get; set; }

        public double TotalPlanArea => Plates.Sum(p => Math.Round(p.Area, 3));
    }
}

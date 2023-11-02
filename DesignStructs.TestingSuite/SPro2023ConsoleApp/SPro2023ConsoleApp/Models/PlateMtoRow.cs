using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.StaadPro.Interactivity.Entities;

using SPro2023ConsoleApp.Base;
using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Models
{
    public class PlateMtoRow : MTOTableRow, IMTOTableRow
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

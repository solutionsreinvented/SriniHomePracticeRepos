using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.StaadPro.Interactivity.Entities;

using SPro2023ConsoleApp.Base;
using SPro2023ConsoleApp.Interfaces;

namespace SPro2023ConsoleApp.Models
{
    public class SectionMtoRow : MTOTableRow, IMTOTableRow
    {
        public SectionMtoRow()
        {
            Beams = new HashSet<Beam>();
        }

        public HashSet<Beam> Beams { get; set; }

        public string PropertyName { get; set; }

        public double SectionalArea { get; set; }

        public double TotalLength => Beams.Sum(b => Math.Round(b.Length, 3));

    }
}

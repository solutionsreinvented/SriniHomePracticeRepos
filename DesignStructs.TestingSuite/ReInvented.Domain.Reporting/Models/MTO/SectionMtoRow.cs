using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.Reporting.Base;
using ReInvented.Domain.Reporting.Interfaces;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Reporting.Models
{
    public class SectionMtoRow : MtoTableRow, IMtoTableRow
    {
        #region Default Constructor

        public SectionMtoRow()
        {
            Beams = new HashSet<Beam>();
        }

        #endregion

        #region Public Properties

        public HashSet<Beam> Beams { get; set; }

        public string PropertyName { get; set; }

        public double SectionalArea { get; set; }

        public double TotalLength => Beams.Sum(b => Math.Round(b.Length, 3));

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using DevDrive.Interfaces;

using ReInvented.Sections.Domain.Models;
using ReInvented.StaadPro.Interop.Entities;

namespace DevDrive.Models
{
    public abstract class MtoTableRow : IMtoTableRow
    {
        #region Parameterized Constructor

        public MtoTableRow(int propertyId)
        {
            PropertyId = propertyId;
        }

        #endregion

        #region Public Properties

        public int PropertyId { get; private set; }

        public double TotalWeight { get; set; }

        public MaterialGrade MaterialGrade { get; set; }

        #endregion
    }

    public class PlateMtoRow : MtoTableRow, IMtoTableRow
    {
        #region Parameterized Constructor

        public PlateMtoRow(int propertyId) : base(propertyId)
        {
            Plates = new HashSet<Plate>();
        }

        #endregion

        #region Public Properties

        public HashSet<Plate> Plates { get; private set; }

        public double Thickness { get; set; }

        public double TotalPlanArea => Plates.Sum(p => Math.Round(p.Area, 3));

        #endregion
    }

    public class SectionMtoRow : MtoTableRow, IMtoTableRow
    {
        #region Parameterized Constructor

        public SectionMtoRow(int propertyId) : base(propertyId)
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

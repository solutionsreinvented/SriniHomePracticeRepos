using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Reporting.Models
{
    public sealed class MaterialTakeOff
    {
        #region Facilitating Properties

        [JsonIgnore]
        public Dictionary<int, SectionMtoRow> SectionsRows { get; set; }

        [JsonIgnore]
        public Dictionary<int, PlateMtoRow> PlatesRows { get; set; }

        [JsonIgnore]
        public HashSet<EntityGroup<Beam>> BeamEntityGroups { get; set; }

        [JsonIgnore]
        public HashSet<EntityGroup<Plate>> PlateEntityGroups { get; set; }

        [JsonIgnore]
        public double SectionsTotalWeight => SectionsRows == null ? 0.0 : Math.Round(SectionsRows.Values.Sum(v => v.TotalWeight), 3);

        [JsonIgnore]
        public double PlatesTotalWeight => PlatesRows == null ? 0.0 : Math.Round(PlatesRows.Values.Sum(v => v.TotalWeight), 3);

        [JsonIgnore]
        public double TotalWeight => Math.Round(SectionsTotalWeight + PlatesTotalWeight, 3);

        #endregion

        #region Public Properties

        public OverallSummary OverallSummary { get; set; }

        public PropertyWiseSummary PropertyWiseSummary { get; set; }

        public Contingencies Contingencies { get; set; }

        #endregion
    }
}

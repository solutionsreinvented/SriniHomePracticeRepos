using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;

using SPro2023ConsoleApp.Services;

namespace SPro2023ConsoleApp.Models
{
    public class MaterialTakeOff
    {
        #region Private Constructor

        private MaterialTakeOff()
        {
            SectionsRows = new Dictionary<int, SectionMtoRow>();
            PlatesRows = new Dictionary<int, PlateMtoRow>();
        }

        #endregion

        #region Public Properties

        public Dictionary<int, SectionMtoRow> SectionsRows { get; private set; }

        public Dictionary<int, PlateMtoRow> PlatesRows { get; private set; }

        public double SectionsTotalWeight => Math.Round(SectionsRows.Values.Sum(v => v.TotalWeight), 3);

        public double PlatesTotalWeight => Math.Round(PlatesRows.Values.Sum(v => v.TotalWeight), 3);

        public double TotalWeight => Math.Round(SectionsTotalWeight + PlatesTotalWeight, 3);

        #endregion

        #region Public Functions

        public static MaterialTakeOff Generate(StaadModelWrapper wrapper)
        {
            int beamGroupsCount = wrapper.Geometry.GetGroupCount(GroupType.Beams);

            object groups = new string[beamGroupsCount];

            wrapper.Geometry.GetGroupNames(GroupType.Beams, ref groups);


            if (wrapper == null)
            {
                throw new ArgumentNullException($"{nameof(wrapper)} shall not be null.");
            }

            MaterialTakeOff takeOff = new MaterialTakeOff()
            {
                SectionsRows = SectionsMaterialTakeOffService.Generate(wrapper),
                PlatesRows = PlatesMaterialTakeOffService.Generate(wrapper)
            };

            return takeOff;
        } 

        #endregion
    }
}

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
            Sections = new Dictionary<int, BeamMTOTableRow>();
            Plates = new Dictionary<int, PlateMTOTableRow>();
        }

        #endregion

        #region Public Properties

        public Dictionary<int, BeamMTOTableRow> Sections { get; private set; }

        public Dictionary<int, PlateMTOTableRow> Plates { get; private set; }

        public double TotalWeight => Math.Round(Sections.Values.Sum(v => v.TotalWeight) + Plates.Values.Sum(v => v.TotalWeight), 3);

        #endregion

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
                Sections = SectionsMaterialTakeOffService.Generate(wrapper),
                Plates = PlatesMaterialTakeOffService.Generate(wrapper)
            };

            return takeOff;
        }
    }
}

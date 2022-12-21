using System.Collections.Generic;

using Techtural.Loading.Enums;
using Techtural.Loading.Interfaces;

namespace Techtural.Loading.Models
{
    public class LoadCase : ILoadCase
    {
        public LoadCase(int id)
        {
            Id = id;
            LoadType = LoadType.None;
            LoadCaseType = LoadCaseType.PrimaryLoad;
            NodeDisplacements = new HashSet<NodeDisplacements>();
            BeamForces = new HashSet<BeamForces>();
        }

        public string Title { get; set; }

        public int Id { get; private set; }

        public HashSet<NodeDisplacements> NodeDisplacements { get; set; }

        public HashSet<BeamForces> BeamForces { get; set; }

        public LoadCaseType LoadCaseType { get; set; }

        public LoadType LoadType { get; set; }
    }
}

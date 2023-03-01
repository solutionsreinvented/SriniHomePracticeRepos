using System.Collections.Generic;

using Techtural.Loading.Enums;
using Techtural.Loading.Models;

namespace Techtural.Loading.Interfaces
{
    public interface ILoadCase
    {
        string Title { get; }

        int Id { get; }

        LoadCaseType LoadCaseType { get; }

        LoadType LoadType { get; }

        HashSet<NodeDisplacements> NodeDisplacements { get; }

        HashSet<BeamForces> BeamForces { get; }
    }
}

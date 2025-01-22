using System.Collections.Generic;
using System.Linq;

using PlateMeshing.Base;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Entities;

namespace PlateMeshing.Models
{
    public class ParametricSurfaceMeshData : ParametricSurfaceMesh, IEntity
    {
        #region Parameterized Constructor

        public ParametricSurfaceMeshData(int id, IEnumerable<Node> generatedNodes, IEnumerable<Plate> generatedPlates)
        {
            Id = id;
            GeneratedNodes = generatedNodes.ToHashSet();
            GeneratedElements = generatedPlates.ToHashSet();
        } 

        #endregion

        #region Properties

        public HashSet<Node> GeneratedNodes { get; private set; }

        public HashSet<Plate> GeneratedElements { get; private set; }

        #endregion
    }
}

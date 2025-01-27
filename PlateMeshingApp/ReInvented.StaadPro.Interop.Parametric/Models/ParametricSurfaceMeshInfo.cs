using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Parametric.Base;

namespace ReInvented.StaadPro.Interop.Parametric.Models
{
    public class ParametricSurfaceMeshInfo : ParametricSurfaceMesh, IEntity
    {
        #region Parameterized Constructor

        public ParametricSurfaceMeshInfo(int id, int nodeCount, int elementCount)
        {
            Id = id;
            NodeCount = nodeCount;
            ElementCount = elementCount;
        }

        #endregion

        #region Properties

        public int NodeCount { get; private set; }

        public int ElementCount { get; private set; }

        #endregion
    }
}


using ReInvented.Shared.Interfaces;

namespace PlateMeshing.Base
{
    public abstract class ParametricSurfaceMesh : IEntity
    {
        #region Properties

        public int Id { get; protected set; } 

        #endregion
    }
}

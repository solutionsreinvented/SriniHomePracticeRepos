using ReIn.VectorsPractice.Base;
using ReIn.VectorsPractice.Enums;
using ReIn.VectorsPractice.Factories;
using ReIn.VectorsPractice.Interfaces;

namespace ReIn.VectorsPractice.Models
{
    public class PortalFrameGrid : FrameGrid, IFrameGrid
    {
        public PortalFrameGrid()
        {
            GridType = FrameGridType.PortalFrameGrid;
        }

        protected override void UpdateType()
        {
            this = FrameGridFactory.Create(GridType);
        }
    }
}

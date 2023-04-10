using ReIn.VectorsPractice.Base;
using ReIn.VectorsPractice.Enums;
using ReIn.VectorsPractice.Interfaces;

namespace ReIn.VectorsPractice.Models
{
    public class BridgeFrameGrid : FrameGrid, IFrameGrid
    {

        public BridgeFrameGrid()
        {
            GridType = FrameGridType.BridgeFrameGrid;
        }

        public BridgeFrameGrid(int id) : base(id)
        {

        }

        protected override void UpdateType()
        {
            throw new System.NotImplementedException();
        }
    }
}

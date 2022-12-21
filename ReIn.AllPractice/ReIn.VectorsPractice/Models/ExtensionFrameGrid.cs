using ReIn.VectorsPractice.Base;
using ReIn.VectorsPractice.Enums;
using ReIn.VectorsPractice.Interfaces;

namespace ReIn.VectorsPractice.Models
{
    public class ExtensionFrameGrid : FrameGrid, IFrameGrid
    {
        public ExtensionFrameGrid()
        {
            GridType = FrameGridType.ExtensionFrameGrid;
        }
    }
}

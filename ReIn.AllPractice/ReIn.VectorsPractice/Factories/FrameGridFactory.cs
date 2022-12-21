using ReIn.VectorsPractice.Enums;
using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Factories
{
    public static class FrameGridFactory
    {
        public static IFrameGrid Create(FrameGridType ofType)
        {
            if (ofType == FrameGridType.PortalFrameGrid)
            {
                return new PortalFrameGrid();
            }
            else if (ofType == FrameGridType.BridgeFrameGrid)
            {
                return new BridgeFrameGrid();
            }
            else
            {
                return new ExtensionFrameGrid();
            }
        }
    }
}

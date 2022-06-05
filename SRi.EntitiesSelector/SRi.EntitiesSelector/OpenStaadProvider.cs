using Microsoft.VisualBasic;
using OpenSTAADUI;

namespace SRi.EntitiesSelector
{
    public class OpenStaadProvider
    {
        public static void Instantiate() => OpenStaad = Interaction.GetObject(null, "StaadPro.OpenSTAAD") as OpenSTAAD;

        public static OpenSTAAD OpenStaad { get; set; }

        public static OSGeometryUI OSGeometry => OpenStaad?.Geometry;

        public static OSLoadUI OSLoad => OpenStaad?.Load;

        public static OSOutputUI OSOutput => OpenStaad?.Output;

        public static OSCommandsUI OSCommands => OpenStaad?.Command;

        public static OSDesignUI OSDesign => OpenStaad?.Design;

        public static OSPropertyUI OSProperty => OpenStaad?.Property;

        public static OSSupportUI OSSupport => OpenStaad?.Support;

        public static OSTableUI OSTable => OpenStaad?.Table;

        public static OSViewUI OSView => OpenStaad?.View;


    }
}

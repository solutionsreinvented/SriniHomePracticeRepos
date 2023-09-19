namespace ReInvented.ExcelInteropDesign.Interfaces
{
    public interface IGussetParameters
    {
        /// <summary>
        /// Thickness of the gusset plate.
        /// </summary>
        double Tg { get; set; }
        /// <summary>
        /// Length of the connection.
        /// </summary>
        double Lcon { get; set; }
    }
}

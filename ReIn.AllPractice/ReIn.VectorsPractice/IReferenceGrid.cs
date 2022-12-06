using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice
{
    public interface IReferenceGrid
    {
        int Id { get; }
        /// <summary>
        /// Relative spacing of this with respect to the previous frame. This value shall be zero for the first frame.
        /// </summary>
        double Spacing { get; set; }
        /// <summary>
        /// Absolute spacing to this frame with respect to the base reference frame.
        /// </summary>
        double Distance { get; set; }
        /// <summary>
        /// Point to the left of the feed pipe and at the bottom chord level
        /// </summary>
        Node A { get; set; }
        /// <summary>
        /// Point to the right of the feed pipe and at the bottom chord level
        /// </summary>
        Node B { get; set; }
        /// <summary>
        /// Point to the left of the feed pipe and at the top chord level
        /// </summary>
        Node C { get; set; }
        /// <summary>
        /// Point to the right of the feed pipe and at the top chord level
        /// </summary>
        Node D { get; set; }
    }
}
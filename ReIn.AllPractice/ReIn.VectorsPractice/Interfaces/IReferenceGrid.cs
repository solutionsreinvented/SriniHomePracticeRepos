using ReInvented.Thickener.Domain.Entities;

namespace ReIn.VectorsPractice.Interfaces
{
    public interface IReferenceGrid
    {
        int Id { get; }
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
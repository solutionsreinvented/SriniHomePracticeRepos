using System;

using ReIn.VectorsPractice.Base;
using ReIn.VectorsPractice.Interfaces;
using ReIn.VectorsPractice.Models;

namespace ReIn.VectorsPractice.Extensions
{
    public static class FrameGridExtensions
    {
        /// <summary>
        /// Generates the nodes of the actual cross frame based on the width of the frame.
        /// </summary>
        /// <param name="frameGrid"><see cref="FrameGrid"/> for which the grid nodes to be generated.</param>
        /// <param name="referenceWidth">Width based on which the frame nodes to be generated. Typically this is the width
        /// based on which the <see cref="Bridge"/> reference grids are generated.</param>
        public static void GenerateFrameGridNodes(this FrameGrid frameGrid, double referenceWidth, double referenceHeight)
        {



        }
        /// <summary>
        /// Generates the nodes of the actual cross frame based on the width of the frame.
        /// </summary>
        /// <param name="frameGrid"><see cref="IFrameGrid"/> for which the grid nodes to be generated.</param>
        /// <param name="referenceWidth">Width based on which the frame nodes to be generated. Typically this is the width
        /// based on which the <see cref="Bridge"/> reference grids are generated.</param>
        public static void GenerateFrameGridNodes(this IFrameGrid frameGrid, double referenceWidth, double referenceHeight)
        {
            (frameGrid as FrameGrid).GenerateFrameGridNodes(referenceWidth, referenceHeight);
        }
    }
}

using System.Collections.Generic;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace AllCorePracticeApp
{
    public class GeometricCalculations
    {
        public static void Calculate()
        {
            var result = Interpolate.Linear(new List<double>() { 0.5, 0.8 }, new List<double>() { });
        }
    }
}

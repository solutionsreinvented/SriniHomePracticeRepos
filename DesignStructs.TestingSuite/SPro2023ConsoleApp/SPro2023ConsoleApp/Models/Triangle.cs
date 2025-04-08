using System;

using ReInvented.Shared;

namespace SPro2023ConsoleApp.Models
{
    public class Triangle
    {
        #region Parameterized Constructors

        public Triangle(double sideA, double sideB, double angleA)
        {
            CalculatePropertiesForSSA(sideA, sideB, angleA);
        }

        #endregion

        #region Readonly Properties

        public double SideA { get; private set; }

        public double SideB { get; private set; }

        public double SideC { get; private set; }

        public double AngleA { get; private set; }

        public double AngleB { get; private set; }

        public double AngleC { get; private set; }

        #endregion

        #region Private Helpers

        private void CalculatePropertiesForSSA(double sideA, double sideB, double angleA)
        {
            SideA = sideA;
            SideB = sideB;
            AngleA = angleA;

            AngleC = Math.Asin(SideB * Math.Sin(AngleA.Radians()) / SideA).Degrees() - angleA;
            AngleB = 180 - AngleA - AngleC;
            SideC = Math.Sqrt(SideA.Square() + SideB.Square() - 2 * SideA * SideB * Math.Cos(AngleC.Radians()));
        }

        #endregion

    }
}

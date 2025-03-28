using System;

using ReInvented.DroopModifier.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.DroopModifier.Models
{
    public class Reading : ValidatablePropertyStore, IReading, IEquatable<Reading>
    {
        #region Default Constructor

        public Reading()
        {

        }

        #endregion

        #region Parameterized Constructor

        public Reading(double angle, double radius, double droop)
        {
            Angle = angle;
            Radius = radius;
            DroopDelta = droop;
        }

        #endregion

        #region Public Properties

        public double Angle { get => Get<double>(); set => Set(value); }

        public double Radius { get => Get<double>(); set => Set(value); }

        public double DroopDelta { get => Get<double>(); set => Set(value); }

        #endregion

        #region Equality

        public override int GetHashCode() => Tuple.Create(Angle, Radius, DroopDelta).GetHashCode();

        public bool Equals(Reading other)
        {
            if (other == null)
            {
                return false;
            }

            return Angle == other.Angle && Radius == other.Radius && DroopDelta == other.DroopDelta;
        }


        public override bool Equals(object obj) => obj is Reading other && Equals(other);

        public static bool operator ==(Reading left, Reading right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if (left is null || right is null)
            {
                return false;
            }

            return left.Angle == right.Angle && left.Radius == right.Radius && left.DroopDelta == right.DroopDelta;
        }

        public static bool operator !=(Reading left, Reading right) => !(left == right);

        #endregion
    }
}

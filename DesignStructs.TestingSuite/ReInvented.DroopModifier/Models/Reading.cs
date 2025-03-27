using ReInvented.DroopModifier.Interfaces;
using ReInvented.Shared.Stores;

namespace ReInvented.DroopModifier.Models
{
    public class Reading : ValidatablePropertyStore, IReading
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

        //public override int GetHashCode() => this.GetHashCode();

        public bool Equals(Reading reading)
        {
            if (reading == null)
            {
                return false;
            }

            return Angle == reading.Angle && Radius == reading.Radius && DroopDelta == reading.DroopDelta;
        }


        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!(obj is Reading reading))
            {
                return false;
            }

            return Angle == reading.Angle && Radius == reading.Radius && DroopDelta == reading.DroopDelta;
        }

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

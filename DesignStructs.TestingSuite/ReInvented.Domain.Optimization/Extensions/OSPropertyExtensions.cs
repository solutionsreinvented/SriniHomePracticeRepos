using OpenSTAADUI;

namespace ReInvented.Domain.Optimization.Extensions
{
    public interface IGenericSection
    {
        ProfileType SectionType { get; }
    }
    public interface IConventionalSection : IGenericSection
    {
        double Ax { get; set; }
        double D { get; set; }
        double Ix { get; set; }
        double Iy { get; set; }
        double Iz { get; set; }
    }
    public interface IUnconventionalSection : IGenericSection
    {

    }
    public interface ITCISection : IConventionalSection, IGenericSection
    {
        double Bf { get; set; }
        double Tf { get; set; }
        double Tw { get; set; }
    }
    public enum ProfileType
    {

    }

    public class TCISection : ConventionalSection, ITCISection, IConventionalSection, IGenericSection
    {
        #region Parameterized Constructor

        public TCISection(ProfileType profileType) : base(profileType)
        {

        }

        #endregion

        #region Public Properties

        public double Bf { get; set; }

        public double Tf { get; set; }

        public double Tw { get; set; }

        #endregion
    }

    public abstract class ConventionalSection : GenericSection, IConventionalSection, IGenericSection
    {
        #region Parameterized Constructor

        public ConventionalSection(ProfileType profileType) : base(profileType)
        {

        }

        #endregion

        #region Public Properties

        public double Ax { get; set; }
        public double D { get; set; }
        public double Ix { get; set; }
        public double Iy { get; set; }
        public double Iz { get; set; }

        #endregion
    }

    public abstract class GenericSection : IGenericSection
    {
        #region Parameterized Constructor

        public GenericSection(ProfileType profileType)
        {
            SectionType = profileType;
        }

        #endregion

        #region Public Properties

        public ProfileType SectionType { get; private set; }

        #endregion

    }

    public static class OSPropertyExtensions
    {
        public static double[] GetSectionPropertyValues(this OSPropertyUI property, int propertyId)
        {
            int count = property.GetCountofSectionPropertyValuesEx();
            object propertyValues = new double[count];
            object propertyType = 0;

            property.GetSectionPropertyValuesEx(propertyId, ref propertyType, ref propertyValues);
            return (double[])propertyValues;
        }
    }
}

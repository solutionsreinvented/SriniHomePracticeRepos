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
    public interface IITCSection : IConventionalSection, IGenericSection
    {
        double Bf { get; set; }
        double Tf { get; set; }
        double Tw { get; set; }
    }
    public enum ProfileType
    {

    }
    public class GenericSection : IGenericSection
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

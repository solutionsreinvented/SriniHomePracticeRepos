using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.ExcelInterop.Extensions
{
    public static class ReleasesExtensions
    {
        public static int NumberOfTranslationsRestrained(this Releases releases)
        {
            int nRestrainedTranslations = 0;

            nRestrainedTranslations += releases.Fx ? 0 : 1;
            nRestrainedTranslations += releases.Fy ? 0 : 1;
            nRestrainedTranslations += releases.Fz ? 0 : 1;

            return nRestrainedTranslations;
        }

        public static int NumberOfRotationsRestrained(this Releases releases)
        {
            int nRestrainedRotations = 0;

            nRestrainedRotations += releases.Mx ? 0 : 1;
            nRestrainedRotations += releases.My ? 0 : 1;
            nRestrainedRotations += releases.Mz ? 0 : 1;

            return nRestrainedRotations;
        }
    }
}

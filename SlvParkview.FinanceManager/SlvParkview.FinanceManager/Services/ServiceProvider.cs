
using System.IO;
using System.Reflection;

namespace SlvParkview.FinanceManager.Services
{
    public class ServiceProvider
    {
        public static string AppDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string AppDataDirectory => Path.Combine(AppDirectory, "ApplicationData");

        public static string ReportsDirectory => Path.Combine(AppDirectory, "Reports");

        public static string ReportTemplatesDirectory => Path.Combine(ReportsDirectory, "Templates");

        public static string ReportScriptsDirectory => Path.Combine(ReportsDirectory, "Scripts");

        public static string BlockOustandingsReportsDirectory => Path.Combine(ReportsDirectory, "Block Outstandings");

        public static string FlatwiseReportsDirectory => Path.Combine(ReportsDirectory, "Flatwise Reports");

        public static string PaymentsReportsDirectory => Path.Combine(ReportsDirectory, "Payments Reports");

        public static string AssetsDirectory => Path.Combine(AppDataDirectory, "Assets");

        public static string IconsDirectory => Path.Combine(AssetsDirectory, "Icons");

        public static string ImagesDirectory => Path.Combine(AssetsDirectory, "Images");

        public static string LogosDirectory => Path.Combine(AssetsDirectory, "Logos");

        public static string ThemeResourcesDirectory => Path.Combine(AssetsDirectory, "ThemeResources");

    }
}

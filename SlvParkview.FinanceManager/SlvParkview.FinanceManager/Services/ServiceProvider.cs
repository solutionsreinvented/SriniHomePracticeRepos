
using System.IO;
using System.Reflection;

namespace SlvParkview.FinanceManager.Services
{
    public class ServiceProvider
    {
        public static string AppDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string AppDataDirectory => Path.Combine(AppDirectory, "ApplicationData");

        public static string AssetsDirectory => Path.Combine(AppDataDirectory, "Assets");

        public static string IconsDirectory => Path.Combine(AssetsDirectory, "Icons");

        public static string ImagesDirectory => Path.Combine(AssetsDirectory, "Images");

        public static string LogosDirectory => Path.Combine(AssetsDirectory, "Logos");

        public static string ThemeResourcesDirectory => Path.Combine(AssetsDirectory, "ThemeResources");

    }
}

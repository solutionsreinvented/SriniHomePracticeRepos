using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using FeaLinux.App.Services;
using FeaLinux.App.ViewModels;
using FeaLinux.Core.Models;
using FeaLinux.Engine;

namespace FeaLinux.App;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Core model (singleton for the session)
        services.AddSingleton<StructuralModel>();
        services.AddSingleton<AnalysisEngine>(sp =>
            new AnalysisEngine(sp.GetRequiredService<StructuralModel>()));

        // Services
        services.AddSingleton<ProjectService>();
        services.AddSingleton<SelectionService>();
        services.AddSingleton<CommandHistory>();
        services.AddSingleton<AnalysisService>();

        // ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<ViewportViewModel>();
        services.AddSingleton<ModelTreeViewModel>();
        services.AddSingleton<PropertiesViewModel>();

        // Views
        services.AddSingleton<MainWindow>();
    }
}

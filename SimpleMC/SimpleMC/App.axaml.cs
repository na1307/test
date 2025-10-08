using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleMC.Models;
using SimpleMC.ViewModels;
using SimpleMC.Views;

namespace SimpleMC;

public sealed partial class App : Application {
    public static bool IsPortable { get; } = File.Exists(Path.Combine(AppContext.BaseDirectory, "portable.txt"));

    public static string BaseDirectoryPath { get; } = !IsPortable
        ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SimpleMC")
        : AppContext.BaseDirectory;

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted() {
        var c = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, false)
            .AddJsonFile(SettingsService.UserSettingsFilePath, true, true)
            .Build();

        ServiceCollection sc = new();

        sc.Configure<Paths>(c.GetRequiredSection("Paths"));
        sc.Configure<Accounts>(c.GetRequiredSection("Accounts"));
        sc.AddSingleton<SettingsService>();
        sc.AddSingleton<MainWindowViewModel>();
        Ioc.Default.ConfigureServices(sc.BuildServiceProvider());

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit.
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = new MainWindow {
                DataContext = Ioc.Default.GetRequiredService<MainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void DisableAvaloniaDataAnnotationValidation() {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove) {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}

using Avalonia.Styling;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SimpleMC.Models;

namespace SimpleMC.ViewModels;

internal sealed partial class SettingsScreenViewModel : ViewModelBase {
    public SettingsScreenViewModel() {
        var ss = Ioc.Default.GetRequiredService<SettingsService>();
        InstancesPath = ss.Paths.InstancePath;
        OfflineAccountName = ss.Accounts.OfflineAccount ?? string.Empty;
    }

    public IEnumerable<ThemeVariant> Themes => [ThemeVariant.Default, ThemeVariant.Light, ThemeVariant.Dark];

    public ThemeVariant Theme {
        get => Application.Current!.RequestedThemeVariant ?? ThemeVariant.Default;
        set => Application.Current!.RequestedThemeVariant = value;
    }

    [ObservableProperty]
    public partial string InstancesPath { get; set; }

    [ObservableProperty]
    public partial string OfflineAccountName { get; set; }

    [RelayCommand]
    private async Task Save() {
        var ss = Ioc.Default.GetRequiredService<SettingsService>();

        await ss.SavePathsAsync(new() {
            InstancePath = InstancesPath
        });

        await ss.SaveAccountsAsync(new() {
            OfflineAccount = OfflineAccountName
        });
    }
}

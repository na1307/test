using CmlLib.Core;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SimpleMC.Models;

namespace SimpleMC.ViewModels;

internal sealed partial class AddInstanceScreenViewModel : ViewModelBase {
    public AddInstanceScreenViewModel() {
        Name = string.Empty;
        var versions = new MinecraftLauncher().GetAllVersionsAsync().AsTask().GetAwaiter().GetResult();
        AvaliableVersions = versions.Select(v => v.Name);
        Version = versions.LatestReleaseName ?? string.Empty;
    }

    [ObservableProperty]
    public partial string Name { get; set; }

    public IEnumerable<string> AvaliableVersions { get; }

    [ObservableProperty]
    public partial string Version { get; set; }

    [RelayCommand]
    private void Save() {
        Instance.CreateNewInstance(Name, Version);

        Ioc.Default.GetRequiredService<MainWindowViewModel>().CurrentScreen = new InstancesMiddleScreenViewModel(Instance.GetAllInstances());
    }
}

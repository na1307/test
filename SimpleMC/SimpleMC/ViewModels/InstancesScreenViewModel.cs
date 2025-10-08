using AvaloniaDialogs.Views;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SimpleMC.Models;
using System.Diagnostics;

namespace SimpleMC.ViewModels;

internal sealed partial class InstancesScreenViewModel(IEnumerable<Instance> instances) : ViewModelBase {
    public InstancesScreenViewModel() : this([]) { }

    public IEnumerable<Instance> Instances { get; } = instances;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsSelected))]
    public partial Instance? SelectedInstance { get; set; }

    public bool IsSelected => SelectedInstance is not null;

    [RelayCommand]
    private void Add() => Ioc.Default.GetRequiredService<MainWindowViewModel>().CurrentScreen = new AddInstanceScreenViewModel();

    [RelayCommand]
    public async Task Launch() {
        Trace.Assert(SelectedInstance is not null);

        var ss = Ioc.Default.GetRequiredService<SettingsService>();

        if (string.IsNullOrWhiteSpace(ss.Accounts.OfflineAccount)) {
            SingleActionDialog sad = new() {
                Message = "No accounts.",
                ButtonText = "OK"
            };

            await sad.ShowAsync();

            return;
        }

        MinecraftPath path = new(SelectedInstance.GetInstanceDir()) {
            Assets = Path.Combine(App.BaseDirectoryPath, "assets"),
            Library = Path.Combine(App.BaseDirectoryPath, "libraries"),
            Resource = Path.Combine(App.BaseDirectoryPath, "resources"),
            Runtime = Path.Combine(App.BaseDirectoryPath, "runtime"),
            Versions = Path.Combine(App.BaseDirectoryPath, "versions")
        };

        MinecraftLauncher launcher = new(path);

        launcher.FileProgressChanged += (sender, args) => {
            Debug.WriteLine($"Name: {args.Name}");
            Debug.WriteLine($"Type: {args.EventType}");
            Debug.WriteLine($"Total: {args.TotalTasks}");
            Debug.WriteLine($"Progressed: {args.ProgressedTasks}");
        };

        launcher.ByteProgressChanged += (sender, args) => {
            Debug.WriteLine($"{args.ProgressedBytes} bytes / {args.TotalBytes} bytes");
        };

        await launcher.InstallAsync(SelectedInstance.Version);

        var process = await launcher.BuildProcessAsync(SelectedInstance.Version, new() {
            Session = MSession.CreateOfflineSession(ss.Accounts.OfflineAccount),
            MaximumRamMb = 4096
        });

        Debug.WriteLine(process.StartInfo.FileName);
        Debug.WriteLine(process.StartInfo.Arguments);

        ProcessWrapper processWrapper = new(process);
        processWrapper.OutputReceived += (_, e) => Debug.WriteLine(e);

        processWrapper.StartWithEvents();

        var exitCode = await processWrapper.WaitForExitTaskAsync();

        Debug.WriteLine($"Exited with code {exitCode}");
    }

    [RelayCommand]
    public async Task Delete() {
        Trace.Assert(SelectedInstance is not null);

        TwofoldDialog tfd = new() {
            Message = $"Are you sure to delete instance '{SelectedInstance.Name}'?",
            PositiveText = "No",
            NegativeText = "Yes"
        };

        var value = await tfd.ShowAsync();

        if (value is { HasValue: true, Value: false }) {
            Directory.Delete(Path.Combine(Instance.GetInstancesDir().FullName, SelectedInstance.Name), true);

            Ioc.Default.GetRequiredService<MainWindowViewModel>().CurrentScreen = new InstancesMiddleScreenViewModel(Instance.GetAllInstances());
        }
    }

    [RelayCommand]
    public async Task Properties() { }
}

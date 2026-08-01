using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EdgeModTool.Views;

namespace EdgeModTool.ViewModels;

internal sealed partial class MainWindowViewModel : ViewModelBase {
    [ObservableProperty]
    public partial string EdgePath { get; set; } = string.Empty;

    [RelayCommand]
    private async Task Browse() {
        var w = Ioc.Default.GetRequiredService<MainWindow>();

        FilePickerOpenOptions options = new() {
            Title = "Select edge.exe",
            AllowMultiple = false,
            FileTypeFilter = [
                new("edge.exe") {
                    Patterns = ["edge.exe"]
                }
            ]
        };

        var files = await w.StorageProvider.OpenFilePickerAsync(options);

        if (files.Count == 1) {
            EdgePath = files[0].Path.AbsolutePath;
        }
    }
}

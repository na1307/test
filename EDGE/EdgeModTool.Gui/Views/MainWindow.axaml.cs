using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using EdgeModTool.LibTwoTribes;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EdgeModTool.Views;

internal sealed partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        AddHandler(DragDrop.DragOverEvent, DragOverHandler);
        AddHandler(DragDrop.DropEvent, DropHandler);
    }

    private static bool IsSingleFile([NotNullWhen(true)] IStorageItem[]? items) => items?.Length == 1 && File.Exists(items[0].Path.LocalPath);

    [LibraryImport("edgemodtoolcore", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int decompile_text_loc(string textLocPath, string jsonPath);

    [LibraryImport("edgemodtoolcore", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    private static partial int compile_text_loc(string jsonPath, string textLocPath);

    private void DragOverHandler(object? sender, DragEventArgs e) {
        var files = e.DataTransfer.TryGetFiles();

        e.DragEffects = IsSingleFile(files) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void DropHandler(object? sender, DragEventArgs e) {
        var files = e.DataTransfer.TryGetFiles();

        if (!IsSingleFile(files)) {
            return;
        }

        var filePath = files[0].Path.LocalPath;
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e) {
        var files = await StorageProvider.OpenFilePickerAsync(new() {
            AllowMultiple = false
        });

        var localPath = files[0].Path.LocalPath;

        switch (Path.GetFileName(files[0].Path.LocalPath)) {
            case "text.loc":
                var result1 = decompile_text_loc(localPath, Path.Combine(Path.GetDirectoryName(localPath)!, "text.json"));

                if (result1 != 0) {
                    throw new(result1.ToString());
                }

                break;

            case "text.json":
                var result2 = compile_text_loc(localPath, Path.Combine(Path.GetDirectoryName(localPath)!, "text.loc"));

                if (result2 != 0) {
                    throw new(result2.ToString());
                }

                break;

            case "font.bin":
            case "font.xml":
                Compiler.Compile(localPath);

                break;
        }
    }
}

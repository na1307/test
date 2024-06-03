using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using WinVhdInstaller.Models;

namespace WinVhdInstaller.ViewModels.Wizards;

public sealed partial class VhdPageViewModel : WizardPageViewModelBase {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNavigateNext))]
    [NotifyPropertyChangedRecipients]
    private string vhdPath = InstallSettings.Instance.VhdPath;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNavigateNext))]
    [NotifyPropertyChangedRecipients]
    private string vhdSize = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNavigateNext))]
    [NotifyPropertyChangedRecipients]
    private bool? isFixed;

    public override string Title => "VHD settings";
    public override string Description => "Set the path, size and type of the VHD to be created.";
    public override bool CanNavigateNext => Path.GetExtension(VhdPath) is ".vhd" or ".vhdx" && IsFixed is not null && int.TryParse(VhdSize, out _);
    public override bool CanNavigatePrevious => true;

    [RelayCommand]
    private void BrowseButton() {
        SaveFileDialog sfd = new() {
            Title = "VHD path",
            CheckPathExists = true,
            Filter = "VHD file|*.vhd|VHDX file|*.vhdx"
        };

        if (sfd.ShowDialog().GetValueOrDefault()) {
            VhdPath = sfd.FileName;
        }
    }

    [RelayCommand]
    private void FixedButton() => IsFixed = true;

    [RelayCommand]
    private void ExpandedButton() => IsFixed = false;

    partial void OnVhdPathChanged(string? oldValue, string newValue) {
        if (newValue != oldValue) {
            InstallSettings.Instance.VhdPath = newValue;
        }
    }

    partial void OnVhdSizeChanged(string? oldValue, string newValue) {
        if (newValue != oldValue && int.TryParse(VhdSize, out var size)) {
            InstallSettings.Instance.VhdSize = size;
        }
    }

    partial void OnIsFixedChanged(bool? oldValue, bool? newValue) {
        if (newValue != oldValue) {
            InstallSettings.Instance.FixedVhd = newValue!.Value;
        }
    }
}

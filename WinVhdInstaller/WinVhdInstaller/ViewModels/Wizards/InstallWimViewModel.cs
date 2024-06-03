using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Dism;
using Microsoft.Win32;
using System.Globalization;
using WinVhdInstaller.Models;

namespace WinVhdInstaller.ViewModels.Wizards;

public sealed partial class InstallWimViewModel : WizardPageViewModelBase {
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNavigateNext))]
    [NotifyPropertyChangedRecipients]
    private string installWimPath = InstallSettings.Instance.InstallWimPath;

    private IEnumerable<string>? images;

    private IEnumerable<CultureInfo?>? locales;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanNavigateNext))]
    [NotifyPropertyChangedRecipients]
    private string? imageIndexString;

    public override string Title => "Select install.wim path";
    public override string Description => "Select the path to the install.wim file.";
    public override bool CanNavigateNext => File.Exists(InstallWimPath) && Path.GetFileName(InstallWimPath).Equals("install.wim", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(ImageIndexString);
    public override bool CanNavigatePrevious => true;

    public IEnumerable<string>? Images {
        get => images;
        private set => SetProperty(ref images, value);
    }

    [RelayCommand]
    private void BrowseButton() {
        OpenFileDialog ofd = new() {
            Title = "Select install.wim",
            CheckFileExists = true,
            CheckPathExists = true,
            Multiselect = false,
            Filter = "install.wim file|install.wim"
        };

        if (ofd.ShowDialog().GetValueOrDefault()) {
            InstallWimPath = ofd.FileName;
        }
    }

    partial void OnInstallWimPathChanged(string? oldValue, string newValue) {
        if (newValue != oldValue) {
            Images = null;
            locales = null;

            try {
                var infos = DismApi.GetImageInfo(InstallWimPath);
                Images = infos.Select(item => $"[{item.ImageIndex}] {item.ImageName}");
                locales = infos.Select(item => item.DefaultLanguage);
            } catch (DismException) when (!File.Exists(InstallWimPath)) {
                // File is not exist
            }

            InstallSettings.Instance.InstallWimPath = newValue;
        }
    }

    partial void OnImageIndexStringChanged(string? oldValue, string? newValue) {
        if (newValue != oldValue) {
            InstallSettings.Instance.ImageIndex = (Images?.ToList().IndexOf(ImageIndexString ?? string.Empty) ?? -1) + 1;
            InstallSettings.Instance.ImageLocale = locales?.ToList()[InstallSettings.Instance.ImageIndex - 1] ?? CultureInfo.CurrentCulture;
        }
    }
}

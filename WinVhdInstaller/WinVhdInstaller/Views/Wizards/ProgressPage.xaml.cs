using Microsoft.Wim;
using System.Diagnostics;
using WinVhdInstaller.Models;

namespace WinVhdInstaller.Views.Wizards;

/// <summary>
/// ProgressPage.xaml에 대한 상호 작용 논리
/// </summary>
public sealed partial class ProgressPage {
    public ProgressPage() => InitializeComponent();

    protected override async void OnInitialized(EventArgs e) {
        base.OnInitialized(e);
        await process();
        Application.Current.Shutdown();

        static async Task process() {
            var iss = InstallSettings.Instance;

            await File.WriteAllTextAsync("diskpart.txt", $"""
                create vdisk file {iss.VhdPath} maximum {iss.VhdSize} type {(iss.FixedVhd ? "fixed" : "expandable")}
                attach vdisk
                convert gpt
                create partition msr size 16
                create partition primary
                format quick fs ntfs
                assign letter v
                exit
                """);

            await Process.Start("diskpart", "/s diskpart.txt").WaitForExitAsync();

            using var hWim = WimgApi.CreateFile(iss.InstallWimPath, WimFileAccess.Read, WimCreationDisposition.OpenExisting, WimCreateFileOptions.None, WimCompressionType.None);

            WimgApi.SetTemporaryPath(hWim, Environment.GetEnvironmentVariable("temp"));

            using var hImage = WimgApi.LoadImage(hWim, iss.ImageIndex);

            WimgApi.ApplyImage(hImage, "V:\\", WimApplyImageOptions.None);

            await Process.Start("bcdboot", $"V:\\Windows /l {iss.ImageLocale.Name}").WaitForExitAsync();
        }
    }
}

using System.Globalization;

namespace WinVhdInstaller.Models;

public sealed class InstallSettings {
    private InstallSettings() { }

    public string InstallWimPath { get; set; } = string.Empty;
    public int ImageIndex { get; set; }
    public CultureInfo ImageLocale { get; set; } = CultureInfo.CurrentCulture;
    public string VhdPath { get; set; } = string.Empty;
    public int VhdSize { get; set; }
    public bool FixedVhd { get; set; }

    public static InstallSettings Instance { get; } = new();
}

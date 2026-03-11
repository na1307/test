using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace UefiReboot;

internal static class OnLinux {
    private const string Efivars = "/sys/firmware/efi/efivars/";
    private const string Namespace = "8be4df61-93ca-11d2-aa0d-00e098032b8c";
    private static readonly Regex BootDescriptionRegex = new("\a\0\0\0\u0001\0\0\0.\0(?<description>.+?)\0\0\0.\x01.+");

    public static async Task<uint[]> GetFirmwareEntries() {
        var asdf = await File.ReadAllBytesAsync($"{Efivars}BootOrder-{Namespace}");

        return [.. asdf.Skip(4).Distinct().Select(i => (uint)i)];
    }

    public static async Task<string> GetDescription(uint number) {
        var filename = $"Boot{number:D4}-{Namespace}";
        var filecontent = await File.ReadAllBytesAsync(Efivars + filename);

        return BootDescriptionRegex.Match(Encoding.UTF8.GetString(filecontent)).Groups["description"].Value.Trim();
    }

    public static async Task Reboot(uint number) {
        ProcessStartInfo psi = new() {
            FileName = "efibootmgr",
            Arguments = "-n " + number.ToString("X"),
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var efb = Process.Start(psi) ?? throw new("efibootmgr failed.");

        await efb.WaitForExitAsync();

        if (efb.ExitCode != 0) {
            throw new("efibootmgr failed with exit code " + efb.ExitCode);
        }
    }
}

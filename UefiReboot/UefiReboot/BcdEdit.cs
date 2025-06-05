using System.Diagnostics;
using System.Text.RegularExpressions;

namespace UefiReboot;

internal static class BcdEdit {
    private static readonly Regex GuidRegex = new("{[0-9A-F]{8}-(?:[0-9A-F]{4}-){3}[0-9A-F]{12}}", RegexOptions.IgnoreCase);
    private static readonly Regex DescRegex = new(@"description\s+(?<name>.*)", RegexOptions.IgnoreCase);

    public static async Task<Guid[]> GetFirmwareEntries() {
        var output = await RunAndOutput("/enum {fwbootmgr} /v");

        return [.. GuidRegex.Matches(output).Where(m => !m.Value.Contains("a5a30fa2-3d06-4e9f-b5f4-a01df9d1fcba")).Select(m => Guid.Parse(m.Value))];
    }

    public static async Task<string> GetDescription(Guid entryId) {
        var output = await RunAndOutput($"/enum {entryId:B} /v");

        return DescRegex.Match(output).Groups["name"].Value.Trim();
    }

    public static async Task SetFirmwareBootNext(Guid entryId) => _ = await RunAndOutput($"/set {{fwbootmgr}} bootsequence {entryId:B}");

    private static async Task<string> RunAndOutput(string arguments) {
        var be = Process.Start(new PsiBuilder().SetArguments(arguments).Build()) ?? throw new("bcdedit failed.");

        await be.WaitForExitAsync();

        if (be.ExitCode != 0) {
            throw new($"bcdedit failed with exit code {be.ExitCode}.");
        }

        return await be.StandardOutput.ReadToEndAsync();
    }

    private sealed class PsiBuilder {
        private string arguments = string.Empty;

        public PsiBuilder SetArguments(string args) {
            arguments = args;

            return this;
        }

        public ProcessStartInfo Build() => new() {
            FileName = "bcdedit.exe",
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }
}

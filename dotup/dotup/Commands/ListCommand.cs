using Dotup.Json.Registry;
using Dotup.Services;

namespace Dotup.Commands;

[RegisterCommands]
internal sealed class ListCommand {
    /// <summary>
    /// List .NET SDKs
    /// </summary>
    [SuppressMessage("Performance", "CA1822:멤버를 static으로 표시하세요.", Justification = "False positive")]
    public async Task<int> List([FromServices] IRegistryService registryService) {
        DotupRegistry? registry;

        try {
            registry = await registryService.GetRegistryAsync(CancellationToken.None);
        } catch (FileNotFoundException) {
            return DotnetSdkNotInstalled();
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (registry is null) {
            return ExitError(["Something went wrong"]);
        }

        var channels = registry.InstalledChannels;

        foreach (var distinctSdkVersion in channels.Values.Select(c => c.SdkVersion).Distinct().Order(SemVersionComparer.Default)) {
            AnsiConsole.WriteLine(
                $"{distinctSdkVersion} ({string.Join(", ", channels.Where(kv => kv.Value.SdkVersion == distinctSdkVersion).Select(kv => kv.Key))})");
        }

        return 0;
    }
}

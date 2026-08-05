using Dotup.Json.Registry;
using Dotup.Services;

namespace Dotup.Commands;

[RegisterCommands]
internal sealed class UninstallCommand {
    /// <summary>
    /// Uninstall a .NET SDK channel
    /// </summary>
    /// <param name="registryService">Service</param>
    /// <param name="installationService">Service</param>
    /// <param name="channel">
    /// The channel to uninstall. Channel format is can be specified as follows:
    ///   - 10.0.100 (exact version)
    ///   - 10.0.0 (exact release version, the latest SDK for the release)
    ///   - 10.0.1xx (latest patch of major.minor.feature band)
    ///   - 10.0.x (latest feature band and patch of major.minor)
    ///   - 10 (latest version of major version)
    ///   - lts (latest long-term support version)
    ///   - latest (latest stable version)
    ///   - preview (latest preview version)
    /// </param>
    /// <param name="token">Token</param>
    [SuppressMessage("Performance", "CA1822:멤버를 static으로 표시하세요.", Justification = "False positive")]
    [SuppressMessage("Roslynator", "RCS1052:Declare each attribute separately", Justification = "Parameter")]
    public async Task<int> Uninstall(
        [FromServices] IRegistryService registryService,
        [FromServices] ISdkInstallationService installationService,
        [Argument, DotnetVersion(true)] string channel,
        CancellationToken token = default) {
        bool installed;

        try {
            installed = await registryService.IsChannelAlreadyInstalledAsync(channel, token);
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (!installed) {
            return ExitError(["Error: The requested channel is not installed"]);
        }

        DotupRegistry? registry;

        try {
            registry = await registryService.GetRegistryAsync(token);
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (registry is null) {
            return ExitError(["Something went wrong"]);
        }

        var channels = registry.InstalledChannels;
        const string successMessage = "[green]Uninstallation completed successfully![/]";

        if (channels.Count == 1) {
            while (true) {
                AnsiConsole.MarkupLine("[yellow]Currently only one channel is installed.[/]");
                AnsiConsole.Markup("[yellow]Do you want to delete the entire .NET installation folder? (Y: Yes, N: No, C: Cancel) [/]");

                var decision = Console.ReadLine() ?? string.Empty;

                if (decision.Equals("Y", StringComparison.OrdinalIgnoreCase)) {
                    Directory.Delete(InstancesPath, true);
                    File.Delete(RegistryJsonPath);
                    AnsiConsole.MarkupLine(successMessage);

                    return 0;
                }

                if (decision.Equals("N", StringComparison.OrdinalIgnoreCase)) {
                    break;
                }

                if (decision.Equals("C", StringComparison.OrdinalIgnoreCase)) {
                    return 0;
                }
            }
        }

        await installationService.UninstallSdkAsync(channel, registry, true, token);
        AnsiConsole.MarkupLine(successMessage);

        return 0;
    }
}

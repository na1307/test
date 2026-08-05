// ReSharper disable AccessToDisposedClosure

using Dotup.Json.Registry;
using Dotup.Json.ReleasesIndex;
using Dotup.Services;

namespace Dotup.Commands;

[RegisterCommands]
internal sealed class UpdateCommand {
    /// <summary>
    /// Update .NET SDK channels
    /// </summary>
    /// <param name="client">Client</param>
    /// <param name="registryService">Service</param>
    /// <param name="installationService">Service</param>
    /// <param name="channel">
    /// The channel to update. If not specified, all installed channels will be updated.
    /// Channel format is can be specified as follows:
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
    public async Task<int> Update(
        [FromServices] HttpClient client,
        [FromServices] IRegistryService registryService,
        [FromServices] ISdkInstallationService installationService,
        [Argument, DotnetVersion(false)] string? channel = null,
        CancellationToken token = default) {
        if (channel is not null) {
            bool installed;

            try {
                installed = await registryService.IsChannelAlreadyInstalledAsync(channel, token);
            } catch (Exception ex) {
                return ExitError([ex.Message]);
            }

            if (!installed) {
                return ExitError([$"Error: The requested channel {channel} is not installed"]);
            }
        }

        AnsiConsole.Markup("[yellow]Fetching release information...[/]");

        var rij = (await client.GetCachedJsonAsync(ReleasesIndexJson, Djsc.DotnetReleasesIndex, token))!;

        var channels = await rij.ReleasesIndex.ToAsyncEnumerable()
            .Select(async ValueTask<DotnetChannel> (o, ct) => (await client.GetCachedJsonAsync(o.ReleasesJson, Djsc.DotnetChannel, ct))!)
            .ToArrayAsync(token);

        var releases = channels.SelectMany(c => c.Release).ToArray();

        var allSdks = releases
            .Select(r => r.Sdk)
            .Concat(releases.SelectMany(r => r.Sdks ?? []))
            .Distinct()
            .OrderByDescending(s => s.Version, SemVersionComparer.Default)
            .ToArray();

        AnsiConsole.MarkupLine(" [green]Done.[/]");

        DotupRegistry registry;

        try {
            registry = (await registryService.GetRegistryAsync(token))!;
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (channel is not null) {
            if (await UpdateChannel(client, channel, registry, allSdks, channels, registryService, installationService, token) != 0) {
                return 1;
            }
        } else if (await UpdateAllChannels(client, registry, allSdks, channels, registryService, installationService, token) != 0) {
            return 1;
        }

        AnsiConsole.MarkupLine("[green]Update completed successfully![/]");

        return 0;
    }

    private static async Task<int> UpdateAllChannels(
        HttpClient client,
        DotupRegistry registry,
        DotnetSdk[] allSdks,
        DotnetChannel[] channels,
        IRegistryService registryService,
        ISdkInstallationService installationService,
        CancellationToken token) {
        foreach (var channel in registry.InstalledChannels) {
            if (await UpdateChannel(client, channel.Key, registry, allSdks, channels, registryService, installationService, token) != 0) {
                return 1;
            }
        }

        return 0;
    }

    private static async Task<int> UpdateChannel(
        HttpClient client,
        string inputChannelString,
        DotupRegistry registry,
        DotnetSdk[] allSdks,
        DotnetChannel[] channels,
        IRegistryService registryService,
        ISdkInstallationService installationService,
        CancellationToken token) {
        var resolvedSdk = ResolveSdkVersionPattern(inputChannelString, channels, allSdks);

        if (resolvedSdk is null) {
            return ExitError(["Could not find a release matching the specified version."]);
        }

        var installedChannels = registry.InstalledChannels;
        var currentChannel = installedChannels[inputChannelString];
        var currentVersion = currentChannel.SdkVersion;

        if (resolvedSdk.Version == currentVersion) {
            AnsiConsole.MarkupLine("[green]The latest SDK already installed.[/]");

            return 0;
        }

        // Snapshot the old channel state for safe uninstallation later
        DotupRegistryChannel oldChannelSnapshot = new() {
            SdkVersion = currentChannel.SdkVersion,
            RuntimeVersion = currentChannel.RuntimeVersion,
            SdkManifests = [.. currentChannel.SdkManifests]
        };

        var sdkVersion = resolvedSdk.Version;
        var runtimeVersion = resolvedSdk.RuntimeVersion;

        if (runtimeVersion is null) {
            return ExitError(["The SDK does not specify a runtime version."]);
        }

        var existingInstalledChannel = registry.InstalledChannels.Values.FirstOrDefault(c => c.SdkVersion == sdkVersion);

        if (existingInstalledChannel is not null) {
            AnsiConsole.MarkupLine($"[green]SDK {sdkVersion} is already installed.[/]");
            AnsiConsole.MarkupLine($"[purple]Updating channel '{inputChannelString}' registration...[/]");

            await registryService.AddOrModifyRegistryAsync(inputChannelString, registry, sdkVersion, runtimeVersion,
                existingInstalledChannel.SdkManifests, false, token);
        } else {
            var sdkFiles = resolvedSdk.Files;

            var sdkFile = sdkFiles.SingleOrDefault(f => f.RuntimeIdentifier == RuntimeIdentifier && IsAppropriateFile(f));

            if (sdkFile is null) {
                return ExitError(["Could not find a release matching the specified version."]);
            }

            AnsiConsole.Markup("[yellow]Fetching file information...[/]");

            // Get file size information
            long totalSize;

            using (HttpRequestMessage message = new(HttpMethod.Head, sdkFile.Url))
            using (var headResponse = await client.SendAsync(message, token)) {
                headResponse.EnsureSuccessStatusCode();

                totalSize = headResponse.Content.Headers.ContentLength ?? 0;
            }

            if (totalSize == 0) {
                return ExitError(["Something went wrong."]);
            }

            AnsiConsole.MarkupLine(" [green]Done.[/]");
            AnsiConsole.MarkupLine($"[purple]Installing .NET SDK {sdkVersion}...[/]");

            if (!await installationService.InstallSdkAsync(client, inputChannelString, totalSize, sdkFile, registry, sdkVersion, runtimeVersion,
                    token)) {
                return ExitError(["Error: Update failed."]);
            }
        }

        AnsiConsole.Markup($"[yellow]Uninstalling old .NET SDK {currentVersion}...[/]");

        // Create a temporary registry that includes the NEW state (so shared dependencies are preserved)
        // AND the OLD channel as a temporary entry (so it can be targeted for uninstallation).
        DotupRegistry tempRegistry = new() {
            CliVersion = registry.CliVersion,
            InstalledChannels = new(registry.InstalledChannels)
        };

        var tempUninstallKey = Guid.NewGuid().ToString();
        tempRegistry.InstalledChannels[tempUninstallKey] = oldChannelSnapshot;

        await installationService.UninstallSdkAsync(tempUninstallKey, tempRegistry, false, token);
        AnsiConsole.MarkupLine("[green]Done.[/]");

        return 0;
    }
}

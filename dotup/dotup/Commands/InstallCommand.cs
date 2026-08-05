// ReSharper disable AccessToDisposedClosure

using Dotup.Json.Registry;
using Dotup.Json.ReleasesIndex;
using Dotup.Services;

namespace Dotup.Commands;

[RegisterCommands]
internal sealed class InstallCommand {
    /// <summary>
    /// Install a .NET SDK channel
    /// </summary>
    /// <param name="client">Client</param>
    /// <param name="registryService">Service</param>
    /// <param name="installationService">Service</param>
    /// <param name="channel">
    /// The channel to install. If not specified, the version specified in global.json will be used.
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
    public async Task<int> Install(
        [FromServices] HttpClient client,
        [FromServices] IRegistryService registryService,
        [FromServices] ISdkInstallationService installationService,
        [Argument, DotnetVersion(false)] string? channel = null,
        CancellationToken token = default) {
        var resolveGlobalJson = await ResolveVersionFromGlobalJson(token);

        if (channel is null && resolveGlobalJson.SdkVersion is null) {
            return ExitError([
                "No version was specified and global.json was not found. Specify a version explicitly or run from the directory where global.json is located."
            ]);
        }

        var ppChannel = channel ?? resolveGlobalJson.SdkVersion!;
        bool installed;

        try {
            installed = await registryService.IsChannelAlreadyInstalledAsync(ppChannel, token);
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (installed) {
            return ExitError([$"Error: The requested channel {ppChannel} is already installed", "Are you looking for \"dotup update\"?"]);
        }

        var sdkInfo = await ResolveSdkAsync(client, channel, token);

        if (sdkInfo is null) {
            return ExitError(["Something went wrong"]);
        }

        var (sdkVersion, sdkFiles, runtimeVersion) = sdkInfo.Value;

        if (runtimeVersion is null) {
            return ExitError(["The SDK does not specify a runtime version."]);
        }

        if (!Directory.Exists(DotupPath)) {
            Directory.CreateDirectory(DotupPath);
        }

        DotupRegistry registry;

        try {
            registry = (await registryService.GetRegistryAsync(token))!;
        } catch (FileNotFoundException) {
            registry = new();
        } catch (Exception ex) {
            return ExitError([ex.Message]);
        }

        if (registry.InstalledChannels.Values.FirstOrDefault(c => c.SdkVersion == sdkVersion) is { } sdk) {
            AnsiConsole.Markup($"[yellow]The SDK {sdkVersion} is already installed. Just adding channel to registry...[/]");

            await registryService.AddOrModifyRegistryAsync(channel ?? sdk.SdkVersion.ToString(), registry, sdk.SdkVersion, sdk.RuntimeVersion,
                sdk.SdkManifests, false, token);

            return 0;
        }

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

        if (!await installationService.InstallSdkAsync(client, channel, totalSize, sdkFile, registry, sdkVersion, runtimeVersion, token)) {
            return ExitError(["Error: Installation failed."]);
        }

        AnsiConsole.MarkupLine("[green]Installation completed successfully![/]");

        return 0;
    }

    private static Task<(SemVersion, DotnetFile[], SemVersion?)?> ResolveSdkAsync(
        HttpClient client,
        string? channel,
        CancellationToken token = default)
        => channel is not null ? ResolveSdkFromChannelAsync(client, channel, token) : ResolveSdkFromGlobalJsonAsync(client, token);

    private static async Task<(SemVersion, DotnetFile[], SemVersion?)?> ResolveSdkFromChannelAsync(
        HttpClient client,
        string channel,
        CancellationToken token = default) {
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

        var resolvedSdk = ResolveSdkVersionPattern(channel, channels, allSdks);

        if (resolvedSdk is null) {
            ExitError(["Could not find a release matching the specified version."]);

            return null;
        }

        var value = (resolvedSdk.Version, resolvedSdk.Files, resolvedSdk.RuntimeVersion);

        AnsiConsole.MarkupLine(" [green]Done.[/]");

        return value;
    }

    private static async Task<(SemVersion, DotnetFile[], SemVersion?)?> ResolveSdkFromGlobalJsonAsync(
        HttpClient client,
        CancellationToken token = default) {
        var (sdkVersion, globalJsonPath) = await ResolveVersionFromGlobalJson(token);

        if (sdkVersion is null) {
            return null;
        }

        AnsiConsole.MarkupLine($"[cyan]Found global.json: {globalJsonPath}[/]");

        return await ResolveSdkFromChannelAsync(client, sdkVersion, token);
    }

    private static async Task<(string? SdkVersion, string? GlobalJsonPath)> ResolveVersionFromGlobalJson(CancellationToken token = default) {
        var globalJsonPath = FindGlobalJson();

        if (globalJsonPath is null) {
            return (null, null);
        }

        await using FileStream fs = new(globalJsonPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        var globalJson = await JsonSerializer.DeserializeAsync(fs, Djsc.GlobalJson, token);

        if (globalJson?.Sdk?.Version is null) {
            ExitError(["SDK version not found in global.json."]);

            return (null, globalJsonPath);
        }

        return (globalJson.Sdk.Version, globalJsonPath);

        static string? FindGlobalJson() {
            DirectoryInfo? currentDir = new(Environment.CurrentDirectory);

            while (currentDir is not null) {
                var globalJsonPath = Path.Combine(currentDir.FullName, "global.json");

                if (File.Exists(globalJsonPath)) {
                    return globalJsonPath;
                }

                currentDir = currentDir.Parent;
            }

            return null;
        }
    }
}

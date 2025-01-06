using Semver;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

namespace Ndnm;

internal sealed class InstallCommand(HttpClient client) : AsyncCommand<InstallCommand.Settings> {
    private static readonly Uri releasesIndexJsonUrl =
        new("https://dotnetcli.blob.core.windows.net/dotnet/release-metadata/releases-index.json");

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
        using var indexJson = JsonDocument.Parse(await client.GetStringAsync(releasesIndexJsonUrl));

        var channels = getChannelsAsync(client,
            indexJson.RootElement.GetProperty("releases-index").EnumerateArray()
                .Select(release => new Uri(release.GetProperty("releases.json").GetString()!)));

        await foreach (var channel in channels) {
            AnsiConsole.WriteLine($"ChannelVersion: {channel.ChannelVersion}");
            AnsiConsole.WriteLine($"LatestSdk: {channel.LatestSdk}");

            foreach (var release in channel.Releases) {
                AnsiConsole.WriteLine($"Releases.ReleaseVersion: {release.ReleaseVersion}");
                AnsiConsole.WriteLine($"Releases.Sdk.Version: {release.Sdk.Version}");
                AnsiConsole.WriteLine($"Releases.Sdk.DisplayVersion: {release.Sdk.DisplayVersion}");
                AnsiConsole.WriteLine($"Releases.Sdk.RuntimeVersion: {release.Sdk.RuntimeVersion ?? "null"}");

                foreach (var file in release.Sdk.Files) {
                    AnsiConsole.WriteLine($"Releases.Sdk.Files.RuntimeIdentifier: {file.RuntimeIdentifier ?? "null"}");
                    AnsiConsole.WriteLine($"Releases.Sdk.Files.Url: {file.Url}");
                    AnsiConsole.WriteLine($"Releases.Sdk.Files.Sha512Hash: {file.Sha512Hash}");
                }

                foreach (var sdk in release.Sdks ?? []) {
                    AnsiConsole.WriteLine($"Releases.Sdks.Version: {sdk.Version}");
                    AnsiConsole.WriteLine($"Releases.Sdks.DisplayVersion: {sdk.DisplayVersion}");
                    AnsiConsole.WriteLine($"Releases.Sdks.RuntimeVersion: {sdk.RuntimeVersion ?? "null"}");

                    foreach (var file in sdk.Files) {
                        AnsiConsole.WriteLine($"Releases.Sdks.Files.RuntimeIdentifier: {file.RuntimeIdentifier ?? "null"}");
                        AnsiConsole.WriteLine($"Releases.Sdks.Files.Url: {file.Url}");
                        AnsiConsole.WriteLine($"Releases.Sdks.Files.Sha512Hash: {file.Sha512Hash}");
                    }
                }

                AnsiConsole.WriteLine();
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
        }

        return 0;

        static async IAsyncEnumerable<DotNetChannel> getChannelsAsync(HttpClient client, IEnumerable<Uri> uris) {
            foreach (var uri in uris) {
                yield return JsonSerializer.Deserialize(await client.GetStringAsync(uri),
                    ReleasesJsonSerializerContext.Default.DotNetChannel)!;
            }
        }
    }

    internal sealed class Settings : CommandSettings {
        [CommandArgument(0, "<Version>")]
        [Description("A version string.")]
        public required string Version { get; init; }

        public override ValidationResult Validate() {
            try {
                SemVersion.Parse(Version, SemVersionStyles.Any);
            } catch (FormatException) when (Version[^1] is 'x' or 'X') {
                return ValidationResult.Success();
            }

            return ValidationResult.Success();
        }
    }
}

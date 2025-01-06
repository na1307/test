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
            AnsiConsole.WriteLine(channel.ToString());
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

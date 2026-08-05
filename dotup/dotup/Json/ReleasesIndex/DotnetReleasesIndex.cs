namespace Dotup.Json.ReleasesIndex;

internal sealed class DotnetReleasesIndex {
    [JsonPropertyName("releases-index")]
    public required DotnetReleasesIndexObject[] ReleasesIndex { get; init; }
}

namespace Dotup.Json.ReleasesIndex;

internal sealed class DotnetRelease {
    [JsonPropertyName("release-version")]
    public required SemVersion Version { get; init; }

    [JsonPropertyName("sdk")]
    public required DotnetSdk Sdk { get; init; }

    [JsonPropertyName("sdks")]
    public DotnetSdk[]? Sdks { get; init; }
}

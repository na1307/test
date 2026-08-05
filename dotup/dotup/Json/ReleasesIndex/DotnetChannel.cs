namespace Dotup.Json.ReleasesIndex;

internal sealed class DotnetChannel {
    [JsonPropertyName("channel-version")]
    public required Version ChannelVersion { get; init; }

    [JsonPropertyName("latest-release")]
    public required SemVersion LatestRelease { get; init; }

    [JsonPropertyName("latest-sdk")]
    public required SemVersion LatestSdk { get; init; }

    [JsonPropertyName("support-phase")]
    public required SupportPhase SupportPhase { get; init; }

    [JsonPropertyName("release-type")]
    public required ReleaseType ReleaseType { get; init; }

    [JsonPropertyName("releases")]
    public required DotnetRelease[] Release { get; init; }
}

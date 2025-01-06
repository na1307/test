using System.Text.Json.Serialization;

namespace Ndnm;

public sealed record class DotNetChannel {
    [JsonPropertyName("channel-version")]
    public required string ChannelVersion { get; init; }

    [JsonPropertyName("latest-sdk")]
    public required string LatestSdk { get; init; }

    [JsonPropertyName("releases")]
    public required DotNetRelease[] Releases { get; init; }
}

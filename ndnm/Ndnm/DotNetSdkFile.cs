using System.Text.Json.Serialization;

namespace Ndnm;

public sealed record class DotNetSdkFile {
    [JsonPropertyName("rid")]
    public string? RuntimeIdentifier { get; init; }

    [JsonPropertyName("url")]
    public required Uri Url { get; init; }

    [JsonPropertyName("hash")]
    public required string Sha512Hash { get; init; }
}

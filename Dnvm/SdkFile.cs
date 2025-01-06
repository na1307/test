using System.Text.Json.Serialization;

namespace Dnvm;

public sealed record class SdkFile {
    [JsonPropertyName("rid")]
    public string? RuntimeIdentifier { get; init; }

    [JsonPropertyName("url")]
    public required Uri Url { get; init; }

    [JsonPropertyName("hash")]
    public required string Sha512Hash { get; init; }
}

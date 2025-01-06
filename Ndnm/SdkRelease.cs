using System.Text.Json.Serialization;

namespace Ndnm;

public sealed record class SdkRelease {
    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("version-display")]
    public required string DisplayVersion { get; init; }

    [JsonPropertyName("runtime-version")]
    public string? RuntimeVersion { get; init; }

    [JsonPropertyName("files")]
    public required SdkFile[] Files { get; init; }
}

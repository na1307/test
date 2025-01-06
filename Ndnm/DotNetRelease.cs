using System.Text.Json.Serialization;

namespace Ndnm;

public sealed record class DotNetRelease {
    [JsonPropertyName("release-version")]
    public required string ReleaseVersion { get; init; }

    [JsonPropertyName("sdk")]
    public required DotNetSdkRelease Sdk { get; init; }

    [JsonPropertyName("sdks")]
    public DotNetSdkRelease[]? Sdks { get; init; }
}

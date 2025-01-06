using System.Text.Json.Serialization;

namespace Ndnm;

public sealed record class DotNetRelease {
    [JsonPropertyName("release-version")]
    public required string ReleaseVersion { get; init; }

    [JsonPropertyName("sdk")]
    public required SdkRelease Sdk { get; init; }

    [JsonPropertyName("sdks")]
    public SdkRelease[]? Sdks { get; init; }
}

namespace Dotup.Json.Registry;

/// <summary>
/// Represents the SDK and Runtime versions installed for a specific channel.
/// </summary>
internal sealed class DotupRegistryChannel {
    [JsonPropertyName("sdk-version")]
    public required SemVersion SdkVersion { get; set; }

    [JsonPropertyName("runtime-version")]
    public required SemVersion RuntimeVersion { get; set; }

    [JsonPropertyName("sdk-manifests")]
    public required SemVersion[] SdkManifests { get; set; }
}

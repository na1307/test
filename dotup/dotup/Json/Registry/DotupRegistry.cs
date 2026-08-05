namespace Dotup.Json.Registry;

/// <summary>
/// Represents the overall structure of the registry.json file.
/// </summary>
internal sealed class DotupRegistry {
    /// <summary>
    /// The version of the primary dotnet CLI that the dotup shim points to. This typically represents the highest installed SDK version.
    /// </summary>
    [JsonPropertyName("cli-version")]
    public SemVersion? CliVersion { get; set; }

    /// <summary>
    /// A dictionary tracking the SDKs installed via various 'channels' (e.g., "latest", "9", "10.0.101").
    /// </summary>
    [JsonPropertyName("installed-channels")]
    public Dictionary<string, DotupRegistryChannel> InstalledChannels { get; set; } = [];
}

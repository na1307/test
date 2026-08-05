namespace Dotup.Json.ReleasesIndex;

internal sealed class DotnetFile {
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("rid")]
    public string? RuntimeIdentifier { get; init; }

    [JsonPropertyName("url")]
    public required Uri Url { get; init; }

    [JsonPropertyName("hash")]
    public required byte[] Sha512Hash { get; init; }
}

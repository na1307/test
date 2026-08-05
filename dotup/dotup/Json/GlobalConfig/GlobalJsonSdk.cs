namespace Dotup.Json.GlobalConfig;

internal sealed record GlobalJsonSdk {
    [JsonPropertyName("version")]
    public string? Version { get; init; }
}

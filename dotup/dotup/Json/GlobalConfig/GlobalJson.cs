namespace Dotup.Json.GlobalConfig;

internal sealed record GlobalJson {
    [JsonPropertyName("sdk")]
    public GlobalJsonSdk? Sdk { get; init; }
}

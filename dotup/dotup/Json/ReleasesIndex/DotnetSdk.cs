namespace Dotup.Json.ReleasesIndex;

internal sealed class DotnetSdk : IEquatable<DotnetSdk> {
    [JsonPropertyName("version")]
    public required SemVersion Version { get; init; }

    [JsonPropertyName("version-display")]
    public required SemVersion DisplayVersion { get; init; }

    [JsonPropertyName("runtime-version")]
    public SemVersion? RuntimeVersion { get; init; }

    [JsonPropertyName("files")]
    public required DotnetFile[] Files { get; init; }

    public static bool operator ==(DotnetSdk? left, DotnetSdk? right) => Equals(left, right);

    public static bool operator !=(DotnetSdk? left, DotnetSdk? right) => !Equals(left, right);

    public bool Equals(DotnetSdk? other) {
        if (other is null) {
            return false;
        }

        if (ReferenceEquals(this, other)) {
            return true;
        }

        return Version.Equals(other.Version);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || (obj is DotnetSdk other && Equals(other));

    public override int GetHashCode() => Version.GetHashCode();
}

namespace Dvbe;

public readonly record struct ProjectConfiguration(string Name, Platform Platform) {
    public const string DebugName = "Debug";
    public const string ReleaseName = "Release";

    public bool IsValid => !string.IsNullOrWhiteSpace(Name);
}

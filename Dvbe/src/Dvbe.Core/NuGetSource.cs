namespace Dvbe;

public readonly record struct NuGetSource(string Name, Uri Source, NuGetApiVersion ApiVersion) {
    public bool IsValid => !string.IsNullOrWhiteSpace(Name) && Source is not null;
}

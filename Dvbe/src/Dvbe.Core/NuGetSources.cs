using System.Collections;
using System.Collections.ObjectModel;

namespace Dvbe;

public sealed class NuGetSources : Configurations, IEnumerable<NuGetSource> {
    private readonly HashSet<NuGetSource> sources;

    internal NuGetSources() => Sources = new(sources = []);

    public ReadOnlySet<NuGetSource> Sources { get; }

    public NuGetSources AddNuGetGallery() {
        ThrowIfConfigured();
        throwIfAlreadyAdded("nuget.org", new("https://api.nuget.org/v3/index.json", UriKind.Absolute), NuGetApiVersion.V3);

        return this;
    }

    public NuGetSources AddRemote(string name, Uri source, NuGetApiVersion apiVersion = NuGetApiVersion.V3) {
        ThrowIfConfigured();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(source);
        throwIfAlreadyAdded(name, source, apiVersion);

        return this;
    }

    public NuGetSources AddLocal(string name, string path) {
        ThrowIfConfigured();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        throwIfAlreadyAdded(name, new(path, UriKind.Absolute), NuGetApiVersion.Local);

        return this;
    }

    IEnumerator<NuGetSource> IEnumerable<NuGetSource>.GetEnumerator() => sources.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => sources.GetEnumerator();

    private void throwIfAlreadyAdded(string name, Uri source, NuGetApiVersion apiVersion) {
        if (!sources.Add(new(name, source, apiVersion))) {
            throw new InvalidOperationException("Source already added.");
        }
    }
}

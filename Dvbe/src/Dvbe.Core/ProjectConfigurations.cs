using System.Collections;
using System.Collections.ObjectModel;

namespace Dvbe;

public sealed class ProjectConfigurations : Configurations, IEnumerable<ProjectConfiguration> {
    private readonly HashSet<ProjectConfiguration> configurations;

    internal ProjectConfigurations() => Configurations = new(configurations = []);

    public ProjectConfiguration DefaultConfiguration {
        get {
            if (!field.IsValid) {
                throw new InvalidOperationException("The default configuration isn't set.");
            }

            if (!configurations.Contains(field)) {
                throw new InvalidOperationException($"The default configuration value not found in {nameof(Configurations)}.");
            }

            return field;
        }

        private set;
    }

    public ReadOnlySet<ProjectConfiguration> Configurations { get; }

    public ProjectConfigurations AddDefaultConfigurations() {
        ThrowIfConfigured();
        throwIfAlreadyAdded(ProjectConfiguration.DebugName, Platform.AnyCpu);
        throwIfAlreadyAdded(ProjectConfiguration.ReleaseName, Platform.AnyCpu);

        return this;
    }

    public ProjectConfigurations AddDebug(Platform platform) {
        ThrowIfConfigured();
        throwIfAlreadyAdded(ProjectConfiguration.DebugName, platform);

        return this;
    }

    public ProjectConfigurations AddRelease(Platform platform) {
        ThrowIfConfigured();
        throwIfAlreadyAdded(ProjectConfiguration.ReleaseName, platform);

        return this;
    }

    public ProjectConfigurations Add(string name, Platform platform) {
        ThrowIfConfigured();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        throwIfAlreadyAdded(name, platform);

        return this;
    }

    public ProjectConfigurations SetDefault(string name, Platform platform) {
        ThrowIfConfigured();
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        DefaultConfiguration = new(name, platform);

        return this;
    }

    IEnumerator<ProjectConfiguration> IEnumerable<ProjectConfiguration>.GetEnumerator() => configurations.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => configurations.GetEnumerator();

    private void throwIfAlreadyAdded(string name, Platform platform) {
        if (!configurations.Add(new(name, platform))) {
            throw new InvalidOperationException("Source already added.");
        }
    }
}

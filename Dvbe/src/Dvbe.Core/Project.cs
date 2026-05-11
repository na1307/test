namespace Dvbe;

public sealed class Project {
    private readonly NuGetSources pluginSources;
    private readonly PluginStorage pluginStorage;
    private readonly NuGetSources nuGetSources;
    private readonly ProjectConfigurations projectConfigurations;

    internal Project() {
        pluginSources = new();
        pluginStorage = new();
        nuGetSources = new();
        projectConfigurations = new();
    }

    public required string Name { get; set; }

    public required ProjectConfiguration CurrentConfiguration { get; init; }

    public Project ConfigurePluginSources(ConfigureNuGetSources configureNuGetSources) {
        configureNuGetSources(pluginSources);

        pluginSources.IsConfigured = true;

        return this;
    }

    public Project ConfigurePlugins(ConfigurePlugins configurePlugins) {
        configurePlugins(pluginStorage);

        pluginStorage.IsConfigured = true;

        return this;
    }

    public T GetPlugin<T>()
        => pluginStorage.PluginsInternal.TryGetValue(typeof(T), out var value) ? (T)value : throw new ArgumentException("Provided plugin not found.");

    public Project ConfigureNuGetSources(ConfigureNuGetSources configureNuGetSources) {
        configureNuGetSources(nuGetSources);

        nuGetSources.IsConfigured = true;

        return this;
    }

    public Project ConfigureConfigurations(ConfigureConfigurations configureConfigurations) {
        configureConfigurations(projectConfigurations);

        projectConfigurations.IsConfigured = true;

        return this;
    }
}

using Dvbe.Plugin;
using System.Collections;

namespace Dvbe;

public sealed class PluginStorage : Configurations, IEnumerable<IPlugin> {
    internal PluginStorage() {
        PluginsInternal = [];
        Plugins = PluginsInternal.Values;
    }

    public Dictionary<Type, IPlugin>.ValueCollection Plugins { get; }

    internal Dictionary<Type, IPlugin> PluginsInternal { get; }

    public PluginStorage AddCSharp() {
        ThrowIfConfigured();
        throwIfAlreadyAdded((IPlugin)Activator.CreateInstance(Type.GetType("Dvbe.Plugin.CSharpPlugin")!)!);

        return this;
    }

    public PluginStorage Add(string packageId, string pluginId, string version) {
        ThrowIfConfigured();
        ArgumentException.ThrowIfNullOrWhiteSpace(packageId);
        ArgumentException.ThrowIfNullOrWhiteSpace(pluginId);
        ArgumentException.ThrowIfNullOrWhiteSpace(version);

        /* Some plugin add logics... */

        return this;
    }

    IEnumerator<IPlugin> IEnumerable<IPlugin>.GetEnumerator() => Plugins.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Plugins.GetEnumerator();

    private void throwIfAlreadyAdded(IPlugin plugin) {
        if (!PluginsInternal.TryAdd(plugin.GetType(), plugin)) {
            throw new InvalidOperationException("plugin already added.");
        }
    }
}

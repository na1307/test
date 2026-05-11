using System.Collections.ObjectModel;

namespace Dvbe.Plugin.Dotnet;

public abstract class DotnetPlugin<TSelf> : IPlugin where TSelf : DotnetPlugin<TSelf> {
    private readonly HashSet<TargetFramework> targetFrameworksInternal;

    protected DotnetPlugin() => TargetFrameworks = new(targetFrameworksInternal = []);

    [PluginConfig]
    public OutputType OutputType { get; private set; }

    [PluginConfig]
    public ReadOnlySet<TargetFramework> TargetFrameworks { get; }

    public TSelf AddTargetFramework(TargetFramework targetFramework) {
        if (!targetFrameworksInternal.Add(targetFramework)) {
            throw new InvalidOperationException("Already added.");
        }

        return (TSelf)this;
    }

    public TSelf SetOutputType(OutputType outputType) {
        if (outputType == OutputType.NotSet) {
            throw new ArgumentException("Invalid value.", nameof(outputType));
        }

        OutputType = outputType;

        return (TSelf)this;
    }
}

using Dvbe.Plugin.Dotnet;

// ReSharper disable once CheckNamespace
namespace Dvbe;

public static class CSharpPluginExtensions {
    public static Project ConfigureCSharp(this Project project, ConfigureCSharpPlugin configureCSharpPlugin) {
        configureCSharpPlugin(project.GetPlugin<CSharpPlugin>());

        return project;
    }
}

namespace Dvbe.Plugin.Dotnet;

public readonly record struct TargetFramework(string TargetFrameworkMoniker) {
    public static readonly TargetFramework Net100 = new(".NETCoreApp,Version=v10.0");
}

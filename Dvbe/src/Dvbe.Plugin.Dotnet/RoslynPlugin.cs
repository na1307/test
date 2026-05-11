namespace Dvbe.Plugin.Dotnet;

public abstract class RoslynPlugin<TSelf> : DotnetPlugin<TSelf> where TSelf : RoslynPlugin<TSelf>;

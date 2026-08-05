namespace Dotup.Commands;

[RegisterCommands]
internal sealed class EnvCommand {
    /// <summary>
    /// Print
    /// </summary>
    [SuppressMessage("Performance", "CA1822:멤버를 static으로 표시하세요.", Justification = "False positive")]
    public int Env() {
        if (OperatingSystem.IsLinux()) {
            AnsiConsole.WriteLine($"""
                                   export DOTNET_ROOT="{InstancesPath}"
                                   export PATH="{InstancesPath}:$HOME/.dotnet/tools:$PATH"
                                   """);
        }

        return 0;
    }
}

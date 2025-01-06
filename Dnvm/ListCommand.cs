using Spectre.Console.Cli;
using System.Diagnostics;

namespace Dnvm;

internal sealed class ListCommand : AsyncCommand {
    public override async Task<int> ExecuteAsync(CommandContext context) {
        var dotnetPath = Path.Combine(InstancesPath, "dotnet");

        if (!File.Exists(dotnetPath)) {
            throw new FileNotFoundException(".NET is not installed.");
        }

        var dotnetProcess = Process.Start(dotnetPath, "--list-sdks");

        await dotnetProcess.WaitForExitAsync();

        return dotnetProcess.ExitCode;
    }
}

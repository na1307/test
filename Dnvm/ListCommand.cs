using Spectre.Console.Cli;
using System.Diagnostics;

namespace Dnvm;

internal sealed class ListCommand : AsyncCommand {
    public override async Task<int> ExecuteAsync(CommandContext context) {
        var dotnetProcess = Process.Start(Path.Combine(InstancesPath, "dotnet"), "--list-sdks");

        await dotnetProcess.WaitForExitAsync();

        return dotnetProcess.ExitCode;
    }
}

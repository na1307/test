using CliFx.Attributes;
using CliFx.Infrastructure;

namespace CliFx;

[Command]
internal sealed class TestCommandCliFx : ICommand {
    [CommandParameter(0)]
    public required string Name { get; init; }

    [CommandParameter(1)]
    public required int Number { get; init; }

    [CommandOption("verbose")]
    public bool Verbose { get; init; }

    public async ValueTask ExecuteAsync(IConsole console) {
        await console.Output.WriteLineAsync(Verbose ? $"Hello, {Name} at {DateTime.Now:yyyy-MM-dd HH:mm:ss}!" : $"Hello, {Name}!");
        await console.Output.WriteLineAsync($"The number is {Number}");
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Spectre.Console.Cli;

internal sealed class TestCommandSpectreConsoleCli : Command<TestCommandSpectreConsoleCli.Settings> {
    public override int Execute(CommandContext context, Settings settings) {
        AnsiConsole.WriteLine(settings.Verbose ? $"Hello, {settings.Name} at {DateTime.Now:yyyy-MM-dd HH:mm:ss}!" : $"Hello, {settings.Name}!");
        AnsiConsole.WriteLine($"The number is {settings.Number}");

        return 0;
    }

    internal sealed class Settings : CommandSettings {
        [CommandArgument(0, "<name>")]
        [Required]
        public required string Name { get; init; }

        [CommandArgument(1, "<number>")]
        [Required]
        [Range(1, 100)]
        public required int Number { get; init; }

        [CommandOption("--verbose")]
        [DefaultValue(false)]
        public bool Verbose { get; init; }
    }
}

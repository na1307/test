using Spectre.Console.Cli;

namespace Dnvm;

internal static class Program {
    private static Task<int> Main(string[] args) {
        CommandApp app = new();

        app.Configure(config => {
            config.AddCommand<ListCommand>("list").WithDescription("List all locally installed .NET versions");
        });

        return app.RunAsync(args);
    }
}

using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Dnvm;

internal static class Program {
    private static Task<int> Main(string[] args) {
        ServiceCollection sc = new();

        sc.AddHttpClient();

        CommandApp app = new(new TypeRegistrar(sc));

        app.Configure(config => {
            config.AddCommand<ListCommand>("list").WithDescription("List all locally installed .NET versions");
            config.AddCommand<InstallCommand>("install").WithDescription("Install a new .NET version");
        });

        return app.RunAsync(args);
    }
}

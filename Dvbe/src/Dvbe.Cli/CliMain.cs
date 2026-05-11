using System.CommandLine;

namespace Dvbe.Cli;

public static class CliMain {
    public static Task<int> Main(string[] args) {
        Command newCommand = new("new", "Create a new project");

        foreach (var type in (string[])["console", "classlib"]) {
            newCommand.Subcommands.Add(new(type, type));
        }

        Command restoreCommand = new("restore", "Restore a project");
        RootCommand rootCommand = new("dvbe");

        rootCommand.Subcommands.Add(newCommand);
        rootCommand.Subcommands.Add(restoreCommand);

        return rootCommand.Parse(args).InvokeAsync();
    }
}

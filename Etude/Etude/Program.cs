using Etude.Alpm;
using System.CommandLine;

namespace Etude;

internal static class Program {
    private static Task<int> Main(string[] args) {
        RootCommand rootCommand = new("Easy-to-use dependency environment");

        Command updateCommand = new("update", "Update database");

        Option<FileInfo> configOption = new("-c", "Path to custom configuration file") {
            ArgumentHelpName = "Config file"
        };

        updateCommand.AddOption(configOption);

        updateCommand.SetHandler(file => {
            try {
                rootCheck();
                update(file);
            } catch (RootException) {
                Console.WriteLine("Requires root.");

                return Task.FromResult(1);
            }

            return Task.FromResult(0);
        }, configOption);

        rootCommand.AddCommand(updateCommand);

        return rootCommand.InvokeAsync(args);
    }

    private static void rootCheck() {
        if (NativeMethods.GetUID() != 0) {
            throw new RootException();
        }
    }

    private static void update(FileInfo? config = null) {
        PacmanConfig configObj = new(config);

        using AlpmSession session = new(configObj);

        session.UpdateDb();
    }
}

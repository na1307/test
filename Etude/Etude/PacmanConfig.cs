using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Etude;

public sealed class PacmanConfig {
    private const string defaultConfigPath = "/etc/pacman.conf";

    public string RootDir { get; } = "/";

    public string DbPath { get; } = "/var/lib/pacman";

    public Architecture Architecture { get; } = RuntimeInformation.OSArchitecture;

    public ReadOnlyDictionary<string, HashSet<Uri>> RepoServers { get; }

    public PacmanConfig(FileInfo? config) {
        var content = File.ReadAllLines(config?.FullName ?? defaultConfigPath)
            .Select(l => l.Trim())
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.StartsWith('#'));

        var currentSection = string.Empty;
        Dictionary<string, HashSet<Uri>> repoServers = [];

        foreach (var line in content) {
            if (line.StartsWith('[') && line.EndsWith(']')) {
                currentSection = line.Trim('[', ']');
            } else if (line.Contains('=')) {
                switch (currentSection) {
                    case "options":
                        var optionKeyValue = line.Split('=', 2,
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        switch (optionKeyValue[0]) {
                            case "RootDir":
                                RootDir = optionKeyValue[1];
                                break;

                            case "DBPath":
                                DbPath = optionKeyValue[1];
                                break;

                            case "Architecture":
                                Architecture = ArchitectureExtensions.FromAlpmArchitecture(optionKeyValue[1]);
                                break;
                        }

                        break;

                    default:
                        if (!repoServers.TryGetValue(currentSection, out var value)) {
                            value = [];
                            repoServers.Add(currentSection, value);
                        }

                        var serverKeyValue = line.Split('=', 2,
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                        switch (serverKeyValue[0]) {
                            case "Server":
                                value.Add(new(serverKeyValue[1]));
                                break;

                            case "Include":
                                foreach (var server in File.ReadAllLines(serverKeyValue[1])
                                             .Select(l => l.Trim())
                                             .Where(l => !string.IsNullOrWhiteSpace(l) && l.StartsWith("Server ="))
                                             .Select(l => new Uri(
                                                 l["Server = ".Length..]
                                                     .Replace("$repo", currentSection)
                                                     .Replace("$arch", Architecture.ToAlpmArchitecture())
                                             ))) {
                                    value.Add(server);
                                }

                                break;

                            case "SigLevel":
                                break;
                        }

                        break;
                }
            }
        }

        RepoServers = repoServers.AsReadOnly();
    }
}

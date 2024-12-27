namespace Etude.Alpm;

[Flags]
public enum DbUsage {
    Sync = 1,
    Search = 1 << 1,
    Install = 1 << 2,
    Upgrade = 1 << 3,
    All = Sync | Search | Install | Upgrade
}

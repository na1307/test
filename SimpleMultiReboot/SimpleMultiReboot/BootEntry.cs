namespace SimpleMultiReboot;

internal readonly record struct BootEntry {
    private readonly Guid identifier;

    public BootEntry(EntryType type, Guid identifier, string description) {
        Type = type;
        this.identifier = identifier;
        Description = description;
    }

    public EntryType Type { get; }

    public string TypeString
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one
        => Type switch {
            EntryType.FirmwareApplication => "Firmware Application",
            EntryType.WindowsBootLoader => "Windows Boot Loader",
            _ => throw new ArgumentOutOfRangeException()
        };
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one

    public string Identifier => identifier.ToString("B");

    public string Description { get; }
}

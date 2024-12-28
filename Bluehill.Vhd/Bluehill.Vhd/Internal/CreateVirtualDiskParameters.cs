namespace Bluehill.Vhd.Internal;

[StructLayout(LayoutKind.Sequential)]
internal struct CreateVirtualDiskParameters {
    public readonly int Version = 1;
    public Guid UniqueId;
    public ulong MaximumSize;
    public uint BlockSizeInBytes;
    public uint SectorSizeInBytes;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string? ParentPath;

    [MarshalAs(UnmanagedType.LPWStr)]
    public string? SourcePath;

    public CreateVirtualDiskParameters() { }
}

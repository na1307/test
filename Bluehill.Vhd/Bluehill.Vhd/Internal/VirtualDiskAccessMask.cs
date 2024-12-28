namespace Bluehill.Vhd.Internal;

[Flags]
internal enum VirtualDiskAccessMask {
    None = 0x00000000,
    AttachRO = 0x00010000,
    AttachRW = 0x00020000,
    Detach = 0x00040000,
    GetInfo = 0x00080000,
    Read = AttachRO | Detach | GetInfo,
    Create = 0x00100000,
    Metaops = 0x00200000,
    Writable = AttachRW | Create | Metaops,
    All = Read | Writable
}

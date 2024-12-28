namespace Bluehill.Vhd.Internal;

[Flags]
internal enum AttachVirtualDiskOptions {
    None = 0x00000000,
    ReadOnly = 0x00000001,
    NoDriveLetter = 0x00000002,
    PermanentLifetime = 0x00000004,
    NoLocalHost = 0x00000008,
    NoSecurityDescriptor = 0x00000010,
    BypassDefaultEncryptionPolicy = 0x00000020,
    NonPnp = 0x00000040,
    RestrictedRange = 0x00000080,
    SinglePartition = 0x00000100,
    RegisterVolume = 0x00000200,
    AtBoot = 0x00000400
}

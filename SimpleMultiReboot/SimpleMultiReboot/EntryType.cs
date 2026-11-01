namespace SimpleMultiReboot;

#pragma warning disable CA1028 // Enum Storage should be Int32
internal enum EntryType : uint {
#pragma warning restore CA1028 // Enum Storage should be Int32
    FirmwareApplication = 0x101fffff,
    WindowsBootLoader = 0x10200003
}

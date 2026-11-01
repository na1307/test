namespace SimpleMultiReboot;

#pragma warning disable CA1028 // Enum Storage should be Int32
internal enum BcdElementTypes : uint {
#pragma warning restore CA1028 // Enum Storage should be Int32
    Description = 0x12000004,
    BootSequence = 0x24000002
}

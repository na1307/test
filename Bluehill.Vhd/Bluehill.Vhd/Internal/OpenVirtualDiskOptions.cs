namespace Bluehill.Vhd.Internal;

[Flags]
internal enum OpenVirtualDiskOptions {
    None = 0x00000000,
    NoParents = 0x00000001,
    BlankFile = 0x00000002,
    BootDrive = 0x00000004,
    CachedIO = 0x00000008,
    CustomDiffChain = 0x00000010,
    ParentCachedIo = 0x00000020,
    VhdsetFileOnly = 0x00000040,
    IgnoreRelativeParentLocator = 0x00000080,
    NoWriteHardening = 0x00000100,
    SupportCompressedVolumes = 0x00000200,
    SupportSparseFilesAnyFs = 0x00000400,
    SupportEncryptedFiles = 0x00000800
}

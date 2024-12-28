namespace Bluehill.Vhd.Internal;

[Flags]
internal enum CreateVirtualDiskOptions {
    None = 0x0,
    FullPhysicalAllocation = 0x1,
    PreventWritesToSourceDisk = 0X2,
    DoNotCopyMetadataFromParent = 0X4,
    CreateBackingStorage = 0X8,
    UseChangeTrackingSourceLimit = 0X10,
    PreserveParentChangeTrackingState = 0X20,
    VhdSetUseOriginalBackingStorage = 0X40,
    SparseFile = 0X80,
    PmemCompatible = 0X100,
    SupportCompressedVolumes = 0x200,
    SupportSparseFilesAnyFs = 0x400
}

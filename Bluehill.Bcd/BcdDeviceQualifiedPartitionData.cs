namespace Bluehill.Bcd;

/// <summary>
/// Represents a qualified partition device element.
/// </summary>
public sealed record class BcdDeviceQualifiedPartitionData : BcdDeviceData {
    internal BcdDeviceQualifiedPartitionData(DeviceType dType, BcdObject? addOptions, PartitionStyle partitionStyle, string diskSignature, string partitionIdentifier) : base(dType, addOptions) {
        PartitionStyle = partitionStyle;
        DiskSignature = diskSignature;
        PartitionIdentifier = partitionIdentifier;
    }

    /// <summary>
    /// The partition style.
    /// </summary>
    public PartitionStyle PartitionStyle { get; }

    /// <summary>
    /// If the PartitionStyle property is a GUID partition table (GPT), the DiskSignature property is the disk signature as a GUID in string format (for example, "{7c69a706-eda5-11dd-bc09-001e4ce28b8f}"). If the PartitionStyle property is a master boot record (MBR), the DiskSignature property is the decimal MBR disk signature in string format (for example, "402653184" for 0x18000000).
    /// </summary>
    public string DiskSignature { get; }

    /// <summary>
    /// If the PartitionStyle property is GPT, the PartitionIdentifier property is the partition signature as a GUID in string format (for example, "{6efb52bf-1766-41db-a6b3-0ee5eff72bd7}" ). If the PartitionStyle property is MBR, the PartitionIdentifier property is the decimal MBR partition offset in string format (for example, "82837504" for 0x4F00000).
    /// </summary>
    public string PartitionIdentifier { get; }
}

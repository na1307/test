namespace Bluehill.Bcd;

/// <summary>
/// The device type.
/// </summary>
public enum DeviceType {
    /// <summary>
    /// Device that initiated the boot.
    /// </summary>
    BootDevice = 1,

    /// <summary>
    /// Disk partition.
    /// </summary>
    PartitionDevice,

    /// <summary>
    /// File that contains file system metadata and is treated as a device.
    /// </summary>
    FileDevice,

    /// <summary>
    /// Ramdisk.
    /// </summary>
    RamdiskDevice,

    /// <summary>
    /// Unknown.
    /// </summary>
    UnknownDevice,

    /// <summary>
    /// Qualified disk partition.
    /// </summary>
    QualifiedPartition,

    /// <summary>
    /// This value is not used.
    /// </summary>
    LocateDevice,

    /// <summary>
    /// Locate device.
    /// </summary>
    LocateExDevice
}

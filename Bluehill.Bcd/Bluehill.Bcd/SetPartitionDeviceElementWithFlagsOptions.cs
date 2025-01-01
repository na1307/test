namespace Bluehill.Bcd;

/// <summary>
/// SetPartitionDeviceElementWithFlags flags.
/// </summary>
[Flags]
public enum SetPartitionDeviceElementWithFlagsOptions {
    /// <summary>
    /// Nothing.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Disable virtual hard disk (VHD) detection. Specify this flag if the partition will be used to boot a virtual machine.
    /// </summary>
    DisableVhdDeviceDetection = 0x20
}

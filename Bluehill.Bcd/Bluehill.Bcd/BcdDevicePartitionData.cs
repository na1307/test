namespace Bluehill.Bcd;

/// <summary>
/// Represents a partition device element.
/// </summary>
public sealed record class BcdDevicePartitionData : BcdDeviceData {
    internal BcdDevicePartitionData(DeviceType dType, BcdObject? addOptions, string path) : base(dType, addOptions) => Path = path;

    /// <summary>
    /// The device path.
    /// </summary>
    public string Path { get; }
}

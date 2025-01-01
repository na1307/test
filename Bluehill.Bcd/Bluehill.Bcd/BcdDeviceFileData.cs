namespace Bluehill.Bcd;

/// <summary>
/// Represents a file device element.
/// </summary>
public sealed record class BcdDeviceFileData : BcdDeviceData {
    internal BcdDeviceFileData(DeviceType dType, BcdObject? addOptions, BcdDeviceData parent, string path) : base(dType, addOptions) {
        Parent = parent;
        Path = path;
    }

    /// <summary>
    /// The parent device.
    /// </summary>
    public BcdDeviceData Parent { get; }

    /// <summary>
    /// The device path.
    /// </summary>
    public string Path { get; }
}

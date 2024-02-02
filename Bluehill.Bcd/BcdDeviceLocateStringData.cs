namespace Bluehill.Bcd;

/// <summary>
/// Represents the parent of a virtual hard disk (VHD) file's parent device and specifies the string to search for a VPART+PPART locate device.
/// </summary>
public sealed record class BcdDeviceLocateStringData : BcdDeviceLocateData {
    internal BcdDeviceLocateStringData(DeviceType dType, BcdObject? addOptions, LocateDeviceType type, string path) : base(dType, addOptions, type) => Path = path;

    /// <summary>
    /// The string to search for a VPART+PPART locate device.
    /// </summary>
    public string Path { get; }
}

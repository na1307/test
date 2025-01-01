namespace Bluehill.Bcd;

/// <summary>
/// Represents the element to locate within the virtual hard disk (VHD) and specifies the VHD file's parent device for a VPART locate device.
/// </summary>
public sealed record class BcdDeviceLocateElementChildData : BcdDeviceLocateData {
    internal BcdDeviceLocateElementChildData(DeviceType dType, BcdObject? addOptions, LocateDeviceType type, int element, BcdDeviceData parent) : base(dType, addOptions, type) {
        Element = element;
        Parent = parent;
    }

    /// <summary>
    /// The element to locate within the VHD.
    /// </summary>
    public int Element { get; }

    /// <summary>
    /// The VHD file's parent device.
    /// </summary>
    public BcdDeviceData Parent { get; }
}

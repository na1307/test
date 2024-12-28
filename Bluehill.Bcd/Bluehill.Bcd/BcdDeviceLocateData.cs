namespace Bluehill.Bcd;

/// <summary>
/// Represents the root class of all locate device data types and specifies the locate type of the device.
/// </summary>
public abstract record class BcdDeviceLocateData : BcdDeviceData {
    private protected BcdDeviceLocateData(DeviceType dType, BcdObject? addOptions, LocateDeviceType type) : base(dType, addOptions) => Type = type;

    /// <summary>
    /// The locate device type.
    /// </summary>
    public LocateDeviceType Type { get; }
}

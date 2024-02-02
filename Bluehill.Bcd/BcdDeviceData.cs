namespace Bluehill.Bcd;

/// <summary>
/// The root class of all device data types.
/// </summary>
public abstract record class BcdDeviceData {
    private protected BcdDeviceData(DeviceType dType, BcdObject? addOptions) {
        DeviceType = dType;
        AdditionalOptions = addOptions;
    }

    /// <summary>
    /// The device type.
    /// </summary>
    public DeviceType DeviceType { get; }

    /// <summary>
    /// Additional options for the specified element. Either another object in the store, or <see langword="null"/>.
    /// </summary>
    public BcdObject? AdditionalOptions { get; }
}

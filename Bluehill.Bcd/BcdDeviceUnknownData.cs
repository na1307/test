namespace Bluehill.Bcd;

/// <summary>
/// Represents unknown device data and exposes the data as a BLOB.
/// </summary>
public sealed record class BcdDeviceUnknownData : BcdDeviceData {
    internal BcdDeviceUnknownData(DeviceType dType, BcdObject? addOptions, byte[] data) : base(dType, addOptions) => Data = data;

    /// <summary>
    /// The binary data of the unknown device element.
    /// </summary>
    public byte[] Data { get; }
}

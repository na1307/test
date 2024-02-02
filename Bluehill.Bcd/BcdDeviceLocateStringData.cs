namespace Bluehill.Bcd;

public sealed record class BcdDeviceLocateStringData : BcdDeviceLocateData {
    internal BcdDeviceLocateStringData(DeviceType dType, BcdObject? addOptions, int type, string path) : base(dType, addOptions, type) => Path = path;

    public string Path { get; }
}

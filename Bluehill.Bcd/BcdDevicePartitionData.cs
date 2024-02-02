namespace Bluehill.Bcd;

public sealed record class BcdDevicePartitionData : BcdDeviceData {
    internal BcdDevicePartitionData(DeviceType dType, BcdObject? addOptions, string path) : base(dType, addOptions) => Path = path;

    public string Path { get; }
}

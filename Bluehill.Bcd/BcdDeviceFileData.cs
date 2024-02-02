namespace Bluehill.Bcd;

public sealed record class BcdDeviceFileData : BcdDeviceData {
    internal BcdDeviceFileData(DeviceType dType, BcdObject? addOptions, BcdDeviceData parent, string path) : base(dType, addOptions) {
        Parent = parent;
        Path = path;
    }

    public BcdDeviceData Parent { get; }
    public string Path { get; }
}

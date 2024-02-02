namespace Bluehill.Bcd;

public sealed record class BcdDeviceLocateElementChildData : BcdDeviceLocateData {
    internal BcdDeviceLocateElementChildData(DeviceType dType, BcdObject? addOptions, int type, int element, BcdDeviceData parent) : base(dType, addOptions, type) {
        Element = element;
        Parent = parent;
    }

    public int Element { get; }
    public BcdDeviceData Parent { get; }
}

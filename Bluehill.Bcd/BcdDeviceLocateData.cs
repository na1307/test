namespace Bluehill.Bcd;

public abstract record class BcdDeviceLocateData : BcdDeviceData {
    private protected BcdDeviceLocateData(DeviceType dType, BcdObject? addOptions, int type) : base(dType, addOptions) => Type = type;

    public int Type { get; }
}

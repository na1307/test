namespace Bluehill.Bcd;

public abstract record class BcdDeviceData {
    private protected BcdDeviceData(DeviceType dType, BcdObject? addOptions) {
        DeviceType = dType;
        AdditionalOptions = addOptions;
    }

    public DeviceType DeviceType { get; }
    public BcdObject? AdditionalOptions { get; }
}

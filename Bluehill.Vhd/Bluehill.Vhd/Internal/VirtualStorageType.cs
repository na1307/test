namespace Bluehill.Vhd.Internal;

[StructLayout(LayoutKind.Sequential)]
internal struct VirtualStorageType {
    public static readonly Guid Microsoft = new("{EC984AEC-A0F9-47e9-901F-71415A66345B}");

    public VirtualStorageTypeDevice DeviceId;
    public Guid VendorId;
}

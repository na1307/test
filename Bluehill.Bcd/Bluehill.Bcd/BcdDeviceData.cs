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

    /// <inheritdoc/>
    public virtual bool Equals(BcdDeviceData? other) => ReferenceEquals(this, other) || (other is not null && EqualityContract.Equals(other.EqualityContract) && DeviceType == other.DeviceType && AdditionalOptions == other.AdditionalOptions);

    /// <summary>
    /// Returns the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public override int GetHashCode() => EqualityContract.GetHashCode() + DeviceType.GetHashCode() + (AdditionalOptions?.GetHashCode() ?? 0);
}

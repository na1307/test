namespace Bluehill.Bcd;

/// <summary>
/// GetElementWithFlags flags.
/// </summary>
[Flags]
public enum GetElementWithFlagsOptions {
    /// <summary>
    /// Nothing.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// If this parameter is set and the element is on a partition, the method retrieves the specified element as a <see cref="BcdDeviceQualifiedPartitionData"/> element. If the element is not on a partition, this parameter is ignored.
    /// </summary>
    Qualified = 0x1
}

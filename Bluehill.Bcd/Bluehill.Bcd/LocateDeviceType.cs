namespace Bluehill.Bcd;

/// <summary>
/// The locate device type.
/// </summary>
public enum LocateDeviceType {
    /// <summary>
    /// This locate device type is not used.
    /// </summary>
    Element,

    /// <summary>
    /// A VPART+PPART locate device.
    /// </summary>
    String,

    /// <summary>
    /// A VPART locate device.
    /// </summary>
    ElementChild
}

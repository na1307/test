namespace Bluehill.Bcd;

/// <summary>
/// The partition style.
/// </summary>
public enum PartitionStyle {
    /// <summary>
    /// The partition is described in an MBR.
    /// </summary>
    Mbr,

    /// <summary>
    /// The partition is described in a GPT.
    /// </summary>
    Gpt
}

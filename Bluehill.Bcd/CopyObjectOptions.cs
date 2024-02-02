namespace Bluehill.Bcd;

/// <summary>
/// CopyObject flags. This can be neither, one, or both of the following values.
/// </summary>
[Flags]
public enum CopyObjectOptions {
    /// <summary>
    /// Nothing.
    /// </summary>
    None = 0x0,

    /// <summary>
    /// Creates a new object identifier for the object coped to the target store. If CreateNewId is not set, the object identifier from the source store is used.
    /// </summary>
    CreateNewId = 0x1,

    /// <summary>
    /// Deletes the existing object from the target store before copying the object from the source store. If DeleteExistingObject is not set, the existing object is not deleted from the target store.
    /// </summary>
    DeleteExistingObject = 0x2
}

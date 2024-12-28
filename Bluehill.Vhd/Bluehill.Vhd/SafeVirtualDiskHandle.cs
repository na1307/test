namespace Bluehill.Vhd;

/// <summary>
/// Represents a wrapper class for a virtual disk handle.
/// </summary>
public sealed class SafeVirtualDiskHandle : SafeHandleZeroOrMinusOneIsInvalid {
    /// <summary>
    /// Creates a <see cref="SafeVirtualDiskHandle"/> around a virtual disk handle.
    /// </summary>
    public SafeVirtualDiskHandle() : base(true) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SafeVirtualDiskHandle"/> class.
    /// </summary>
    /// <param name="preexistingHandle">An <see cref="IntPtr"/> object that represents the pre-existing handle to use.</param>
    /// <param name="ownsHandle"><see langword="true"/> to reliably release the handle during the finalization phase; <see langword="false"/> to prevent reliable release (not recommended).</param>
    public SafeVirtualDiskHandle(IntPtr preexistingHandle, bool ownsHandle) : base(ownsHandle) => SetHandle(preexistingHandle);

    /// <inheritdoc/>
    protected override bool ReleaseHandle() => NativeMethods.CloseHandle(handle);
}

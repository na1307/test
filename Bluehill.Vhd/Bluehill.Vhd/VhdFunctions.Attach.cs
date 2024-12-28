namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    /// <summary>
    /// Attaches VHD.
    /// </summary>
    /// <param name="path">Full path of VHD.</param>
    /// <param name="readOnly">Whether to attach as read-only.</param>
    /// <param name="noDriveLetter">Whether to not assign a drive letter.</param>
    /// <exception cref="ArgumentException"><paramref name="path"/> is <see langword="null"/> or whitespace.</exception>
    /// <seealso cref="DetachVhd(string)"/>
    public static void AttachVhd(string path, bool readOnly = false, bool noDriveLetter = false) {
        if (string.IsNullOrEmpty(path) || path.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(path));
        }

        using var handle = GetVhdHandle(path);

        attachVhdCore(handle, readOnly, noDriveLetter);
    }

    /// <summary>
    /// Attaches VHD.
    /// </summary>
    /// <param name="vhdHandle">Handle of VHD.</param>
    /// <param name="readOnly">Whether to attach as read-only.</param>
    /// <param name="noDriveLetter">Whether to not assign a drive letter.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vhdHandle"/> is null.</exception>
    /// <seealso cref="DetachVhd(SafeVirtualDiskHandle)"/>
    public static void AttachVhd(SafeVirtualDiskHandle vhdHandle, bool readOnly = false, bool noDriveLetter = false) {
        if (vhdHandle is null) {
            throw new ArgumentNullException(nameof(vhdHandle));
        }

        attachVhdCore(vhdHandle, readOnly, noDriveLetter);
    }

    private static void attachVhdCore(SafeVirtualDiskHandle handle, bool readOnly, bool noDriveLetter) {
        var options = AttachVirtualDiskOptions.PermanentLifetime;

        if (readOnly) {
            options |= AttachVirtualDiskOptions.ReadOnly;
        }

        if (noDriveLetter) {
            options |= AttachVirtualDiskOptions.NoDriveLetter;
        }

        var result = NativeMethods.AttachVirtualDisk(
            handle,
            IntPtr.Zero,
            options,
            0,
            IntPtr.Zero,
            IntPtr.Zero);

        if (result != 0) {
            throw new VhdOperationFailedException(getMessageFromErrorCode(result));
        }
    }
}

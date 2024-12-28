namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    /// <summary>
    /// Detaches VHD.
    /// </summary>
    /// <param name="path">Fuil path of VHD.</param>
    /// <exception cref="ArgumentException"><paramref name="path"/> is <see langword="null"/> or whitespace.</exception>
    /// <seealso cref="AttachVhd(string, bool, bool)"/>
    public static void DetachVhd(string path) {
        if (string.IsNullOrEmpty(path) || path.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(path));
        }

        using var handle = GetVhdHandle(path);

        detachVhdCore(handle);
    }

    /// <summary>
    /// Detaches VHD.
    /// </summary>
    /// <param name="vhdHandle">Handle of VHD.</param>
    /// <exception cref="ArgumentNullException"><paramref name="vhdHandle"/> is null.</exception>
    /// <seealso cref="attachVhdCore(SafeVirtualDiskHandle, bool, bool)"/>
    public static void DetachVhd(SafeVirtualDiskHandle vhdHandle) {
        if (vhdHandle is null) {
            throw new ArgumentNullException(nameof(vhdHandle));
        }

        detachVhdCore(vhdHandle);
    }

    private static void detachVhdCore(SafeVirtualDiskHandle handle) {
        var result = NativeMethods.DetachVirtualDisk(handle, DetachVirtualDiskOptions.None, 0);

        if (result != 0) {
            throw new VhdOperationFailedException(getMessageFromErrorCode(result));
        }
    }
}

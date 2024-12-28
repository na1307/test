namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    /// <summary>
    /// Returns VHD's handle.
    /// </summary>
    /// <param name="path">Full path of VHD.</param>
    /// <returns>Handle of VHD.</returns>
    /// <exception cref="ArgumentException"><paramref name="path"/> is <see langword="null"/> or whitespace.</exception>
    public static SafeVirtualDiskHandle GetVhdHandle(string path) {
        if (string.IsNullOrEmpty(path) || path.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(path));
        }

        return getVhdHandleCore(path);
    }

    private static SafeVirtualDiskHandle getVhdHandleCore(string path) {
        var vst = getVst(path);
        var result = NativeMethods.OpenVirtualDisk(
            in vst,
            path,
            VirtualDiskAccessMask.All,
            OpenVirtualDiskOptions.None,
            IntPtr.Zero,
            out var handle);

        return result == 0 ? handle : throw new VhdOperationFailedException(getMessageFromErrorCode(result));
    }
}

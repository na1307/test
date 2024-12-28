namespace Bluehill.Vhd;

public static partial class VhdFunctions {
    /// <summary>
    /// Creates new VHD.
    /// </summary>
    /// <param name="path">Full path of VHD.</param>
    /// <param name="size">Size of VHD.</param>
    /// <param name="isFixed">Whether to create a fixed-size VHD.</param>
    /// <returns>Newly created VHD's handle.</returns>
    /// <exception cref="ArgumentException"><paramref name="path"/> is <see langword="null"/> or whitespace.</exception>
    public static SafeVirtualDiskHandle CreateVhd(string path, VhdSize size, bool isFixed = false) {
        if (string.IsNullOrEmpty(path) || path.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(path));
        }

        return createVhdCore(path, null, null, size, isFixed);
    }

    /// <summary>
    /// Creates child VHD from Parent.
    /// </summary>
    /// <param name="child">Full path of child VHD.</param>
    /// <param name="parent">Full path of parent VHD.</param>
    /// <returns>Newly created VHD's handle.</returns>
    /// <exception cref="ArgumentException"><paramref name="child"/> or <paramref name="parent"/> is <see langword="null"/> or whitespace.</exception>
    public static SafeVirtualDiskHandle CreateChildVhd(string child, string parent) {
        if (string.IsNullOrEmpty(child) || child.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(child));
        }

        if (string.IsNullOrEmpty(parent) || parent.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(parent));
        }

        return createVhdCore(child, parent, null, default, false);
    }

    /// <summary>
    /// Creates VHD from source.
    /// </summary>
    /// <param name="destination">Full path of VHD.</param>
    /// <param name="source">Full path of source VHD.</param>
    /// <param name="size">Size of VHD.</param>
    /// <param name="isFixed">Whether to create a fixed-size VHD.</param>
    /// <returns>Newly created VHD's handle.</returns>
    /// <exception cref="ArgumentException"><paramref name="destination"/> or <paramref name="source"/> is <see langword="null"/> or whitespace.</exception>
    public static SafeVirtualDiskHandle CloneVhd(string destination, string source, VhdSize size = default, bool isFixed = false) {
        if (string.IsNullOrEmpty(destination) || destination.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(destination));
        }

        if (string.IsNullOrEmpty(source) || source.Trim().Length == 0) {
            throw new ArgumentException("It cannot be null or whitespace.", nameof(source));
        }

        return createVhdCore(destination, null, source, size, isFixed);
    }

    private static SafeVirtualDiskHandle createVhdCore(string path, string? parent, string? source, VhdSize size, bool isFixed) {
        if (parent is not null && source is not null) {
            throw new ArgumentException("Both");
        }

        if (parent is not null &&
            !Path.GetExtension(path).TrimStart('.').Equals(Path.GetExtension(parent).TrimStart('.'), StringComparison.OrdinalIgnoreCase)) {
            throw new ArgumentException("The parent and child file extensions must be the same.");
        }

        var vst = getVst(path);

        CreateVirtualDiskParameters cvdp = new() {
            UniqueId = Guid.Empty,
            MaximumSize = (ulong)(long)size,
            BlockSizeInBytes = 0,
            SectorSizeInBytes = 512,
            ParentPath = parent,
            SourcePath = source
        };

        var result = NativeMethods.CreateVirtualDisk(
            in vst,
            path,
            VirtualDiskAccessMask.All,
            IntPtr.Zero,
            parent is not null && isFixed ? CreateVirtualDiskOptions.FullPhysicalAllocation : CreateVirtualDiskOptions.None,
            0,
            in cvdp,
            IntPtr.Zero,
            out var handle);

        return result == 0 ? handle : throw new VhdOperationFailedException(getMessageFromErrorCode(result));
    }
}

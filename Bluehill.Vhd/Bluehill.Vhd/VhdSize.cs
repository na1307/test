#if NET7_0_OR_GREATER
using System.Numerics;
#endif

namespace Bluehill.Vhd;

/// <summary>
/// Defines the size of VHD.
/// </summary>
#pragma warning disable CS1591 // 공개된 형식 또는 멤버에 대한 XML 주석이 없습니다.
public readonly struct VhdSize : IEquatable<VhdSize>, IComparable<VhdSize>, IComparable
#if NET7_0_OR_GREATER
    , IComparisonOperators<VhdSize, VhdSize, bool>, IMinMaxValue<VhdSize>
#endif
    {
    /// <summary>
    /// Initializes a new instance of <see cref="VhdSize"/>.
    /// </summary>
    /// <param name="bytes">Bytes of VHD's size.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="bytes"/> is less than 3 MB or greater than 64 TB.</exception>
    public VhdSize(long bytes) {
        const long threeMega = (long)3 * 1024 * 1024;
        const long sixtyFourTera = (long)64 * 1024 * 1024 * 1024 * 1024;

        if (bytes < threeMega) {
            throw new ArgumentOutOfRangeException(nameof(bytes), bytes, "The size must be at least 3 MB.");
        }

        if (bytes > sixtyFourTera) {
            throw new ArgumentOutOfRangeException(nameof(bytes), bytes, "The size must be 64 TB or less.");
        }

        Bytes = bytes;
    }

    /// <summary>
    /// Maximum value of <see cref="VhdSize"/>.
    /// </summary>
    /// <remarks>This property's value is 64 TB.</remarks>
    public static VhdSize MaxValue { get; } = new((long)64 * 1024 * 1024 * 1024 * 1024);

    /// <summary>
    /// Minimum value of <see cref="VhdSize"/>.
    /// </summary>
    /// <remarks>This property's value is 3 MB.</remarks>
    public static VhdSize MinValue { get; } = new((long)3 * 1024 * 1024);

    /// <summary>
    /// Actual value of this <see cref="VhdSize"/>.
    /// </summary>
    public long Bytes { get; }

    public static VhdSize FromMegabytes(long megabyte) => new(megabyte * 1024 * 1024);
    public static VhdSize FromMegabytes(double megabyte) => new((long)(megabyte * 1024 * 1024));

    public static VhdSize FromGigabytes(long gigabyte) => new(gigabyte * 1024 * 1024 * 1024);
    public static VhdSize FromGigabytes(double gigabyte) => new((long)(gigabyte * 1024 * 1024 * 1024));

    public static VhdSize FromTerabytes(long terabyte) => new(terabyte * 1024 * 1024 * 1024 * 1024);
    public static VhdSize FromTerabytes(double terabyte) => new((long)(terabyte * 1024 * 1024 * 1024 * 1024));

    public static VhdSize FromInt64(long value) => new(value);
    public static long ToInt64(VhdSize size) => size.Bytes;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is VhdSize size && Equals(size);

    /// <inheritdoc/>
    public bool Equals(VhdSize other) => Bytes == other.Bytes;

    /// <inheritdoc/>
    public override int GetHashCode() => Bytes.GetHashCode();

    /// <inheritdoc/>
    public int CompareTo(VhdSize other) => Bytes.CompareTo(other.Bytes);

    /// <inheritdoc/>
    public int CompareTo(object? obj) {
        if (obj == null) {
            return 1;
        } else if (obj is VhdSize x) {
            return CompareTo(x);
        } else {
            throw new ArgumentException($"'{nameof(obj)}' is not a VhdSize", nameof(obj));
        }
    }

    /// <inheritdoc/>
    public override string ToString() {
        const long tera = (long)1024 * 1024 * 1024 * 1024;
        const long giga = (long)1024 * 1024 * 1024;
        string bt;

        if (Bytes is >= tera) {
            bt = $"{(decimal)Bytes / 1024 / 1024 / 1024 / 1024:G2} Tetabyte(s)";
        } else if (Bytes is >= giga) {
            bt = $"{(decimal)Bytes / 1024 / 1024 / 1024:G2} Gigabyte(s)";
        } else {
            bt = $"{(decimal)Bytes / 1024 / 1024:G2} Megabyte(s)";
        }

        bt += $" ({Bytes} Byte(s))";

        return bt;
    }

    public static bool operator ==(VhdSize left, VhdSize right) => left.Equals(right);
    public static bool operator !=(VhdSize left, VhdSize right) => !(left == right);
    public static bool operator <(VhdSize left, VhdSize right) => left.CompareTo(right) < 0;
    public static bool operator <=(VhdSize left, VhdSize right) => left.CompareTo(right) <= 0;
    public static bool operator >(VhdSize left, VhdSize right) => left.CompareTo(right) > 0;
    public static bool operator >=(VhdSize left, VhdSize right) => left.CompareTo(right) >= 0;

    public static implicit operator long(VhdSize size) => size.Bytes;
    public static implicit operator VhdSize(long value) => new(value);
}

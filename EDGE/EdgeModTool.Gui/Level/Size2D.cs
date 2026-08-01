using System.Globalization;

namespace EdgeModTool.Level;

internal readonly struct Size2D(ushort width, ushort length) : IXSerializable, IEquatable<Size2D> {
    public readonly ushort Width = width;
    public readonly ushort Length = length;

    public int FlatBytes => Flat.GetBytes(Width, Length);

    public static implicit operator Size2D(Size3D size) => new(size.Width, size.Length);

    public static bool operator ==(Size2D a, Size2D b) => a.Equals(b);

    public static bool operator !=(Size2D a, Size2D b) => !(a == b);

    public bool Equals(Size2D other) => Width == other.Width && Length == other.Length;

    public override bool Equals(object? obj) {
        if (obj is null) {
            return false;
        }

        return obj is Size2D d && Equals(d);
    }

    public override int GetHashCode() {
        unchecked {
            return (Width.GetHashCode() * 397) ^ Length.GetHashCode();
        }
    }

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{Width}x{Length}");

    public void Write(BinaryWriter writer) {
        writer.Write(Width);
        writer.Write(Length);
    }
}

using System.Globalization;

namespace EdgeModTool.Level;

internal readonly struct Size3D(byte height, ushort width, ushort length) : IXSerializable, IComparable, IComparable<Size3D> {
    public readonly ushort Width = width;
    public readonly ushort Length = length;
    public readonly byte Height = height;

    public Size3D(BinaryReader reader) : this(reader.ReadByte(), reader.ReadUInt16(), reader.ReadUInt16()) { }

    public Size2D DefaultLegacyMinimapSize {
        get {
            var p = Width + Length;
            return new((ushort)((p + 9) / 10), (ushort)((p + Height + Height + 9) / 10));
        }
    }

    public static bool operator ==(Size3D a, Size3D b) => a.Equals(b);

    public static bool operator !=(Size3D a, Size3D b) => !a.Equals(b);

    public static Size3D Parse(string str) {
        var numbers = str.Split('x');

        if (numbers.Length != 3 ||
            !ushort.TryParse(numbers[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var width) ||
            !ushort.TryParse(numbers[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var length) ||
            !byte.TryParse(numbers[2].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var height)) {
            throw new FormatException();
        }

        return new(height, width, length);
    }

    public int CompareTo(Size3D other) => (Width * Length * Height).CompareTo(other.Width * other.Length * other.Height);

    public int CompareTo(object? obj) => obj is not Size3D d ? throw new NotSupportedException() : CompareTo(d);

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{Width}x{Length}x{Height}");

    public bool IsBlockInArea(int x, int y, int z) => x >= 0 && y >= 0 && z >= 0 && x < Width && y < Length && z < Height;

    public bool IsBlockInArea(Point3D16 point) => IsBlockInArea(point.X, point.Y, point.Z);

    public bool IsCubeInArea(int x, int y, int z) => x >= 0 && y >= 0 && z > 0 && x < Width && y < Length && z <= Height;

    public bool IsCubeInArea(Point3D16 point) => IsCubeInArea(point.X, point.Y, point.Z);

    public bool Equals(Size3D other) => Width == other.Width && Length == other.Length && Height == other.Height;

    public override bool Equals(object? obj) {
        if (obj is null) {
            return false;
        }

        return obj is Size3D d && Equals(d);
    }

    public override int GetHashCode() => HashCode.Combine(Width, Length, Height);

    public void Write(BinaryWriter writer) {
        writer.Write(Height);
        writer.Write(Width);
        writer.Write(Length);
    }
}

using System.Globalization;

namespace EdgeModTool.Level;

internal readonly struct Point3D16(short x, short y, short z) : IXSerializable, IEquatable<Point3D16>, IComparable, IComparable<Point3D16> {
    public readonly short X = x;
    public readonly short Y = y;
    public readonly short Z = z;

    public Point3D16(BinaryReader reader) : this(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16()) { }

    public static Point3D16 operator +(Point3D16 a, Point3D16 b) => new((short)(a.X + b.X), (short)(a.Y + b.Y), (short)(a.Z + b.Z));

    public static Point3D16 operator -(Point3D16 a, Point3D16 b) => a + -b;

    public static Point3D16 operator -(Point3D16 value) => new((short)-value.X, (short)-value.Y, (short)-value.Z);

    public static bool operator ==(Point3D16 left, Point3D16 right) => left.Equals(right);

    public static bool operator !=(Point3D16 left, Point3D16 right) => !left.Equals(right);

    public static Point3D16 Parse(string str) {
        var numbers = str.Trim('(', ')').Split(',').Select(s => short.Parse(s, CultureInfo.InvariantCulture)).ToArray();

        return new(numbers[0], numbers[1], numbers[2]);
    }

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{X},{Y},{Z}");

    public void Write(BinaryWriter writer) {
        writer.Write(X);
        writer.Write(Y);
        writer.Write(Z);
    }

    public bool Equals(Point3D16 other) => X == other.X && Y == other.Y && Z == other.Z;

    public override bool Equals(object? obj) {
        if (obj is null) {
            return false;
        }

        return obj is Point3D16 d16 && Equals(d16);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    public int CompareTo(object? obj) => obj is not Point3D16 d16 ? throw new NotSupportedException() : CompareTo(d16);

    public int CompareTo(Point3D16 other) {
        var result = X.CompareTo(other.X);

        if (result != 0) {
            return result;
        }

        result = Y.CompareTo(other.Y);

        return result != 0 ? result : Z.CompareTo(other.Z);
    }
}

using System.Globalization;

namespace EdgeModTool.Level;

internal readonly struct Point2D8(byte x, byte y) : IXSerializable, IEquatable<Point2D8> {
    public readonly byte X = x;
    public readonly byte Y = y;

    public Point2D8(BinaryReader reader) : this(reader.ReadByte(), reader.ReadByte()) { }

    public static Point2D8 Parse(string str) {
        var numbers = str.Trim('(', ')').Split(',');

        if (numbers.Length != 2 ||
            !byte.TryParse(numbers[0].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var x) ||
            !byte.TryParse(numbers[1].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var y)) {
            throw new FormatException();
        }

        return new(x, y);
    }

    public bool Equals(Point2D8 other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) {
        if (obj is null) {
            return false;
        }

        return obj is Point2D8 d8 && Equals(d8);
    }

    public override int GetHashCode() {
        unchecked {
            return (X.GetHashCode() * 397) ^ Y.GetHashCode();
        }
    }

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{X},{Y}");

    public void Write(BinaryWriter writer) {
        writer.Write(X);
        writer.Write(Y);
    }
}

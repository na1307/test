using System.Globalization;
using System.Text;

namespace EdgeModTool.LibTwoTribes.Util;

internal struct Vec2(float x, float y) : IEquatable<Vec2> {
    public float X { get; set; } = x;

    public float Y { get; set; } = y;

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);

    public static Vec2 operator -(Vec2 a) => new(-a.X, -a.Y);

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);

    public static Vec2 operator *(Vec2 a, float b) => new(a.X * b, a.Y * b);

    public static Vec2 operator *(float a, Vec2 b) => b * a;

    public static Vec2 operator /(Vec2 a, float b) => new(a.X / b, a.Y / b);

    public static Vec2 FromStream(Stream stream) {
        Vec2 output = default;
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        output.X = br.ReadSingle();
        output.Y = br.ReadSingle();

        return output;
    }

    public static Vec2 Parse(string value) {
        var numbers = value.Trim('(', ')').Split(',').Select(f => float.Parse(f, CultureInfo.InvariantCulture)).ToArray();

        return new(numbers[0], numbers[1]);
    }

    public readonly bool Equals(Vec2 other) => Math.Abs(X - other.X) < 1e-4 && Math.Abs(Y - other.Y) < 1e-4;

    public readonly void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(X);
        bw.Write(Y);
    }

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{X},{Y}");
}

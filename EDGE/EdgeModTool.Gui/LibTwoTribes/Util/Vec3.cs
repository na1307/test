using System.Globalization;
using System.Text;

namespace EdgeModTool.LibTwoTribes.Util;

internal struct Vec3(float x, float y, float z) : IEquatable<Vec3> {
    public float X { get; set; } = x;

    public float Y { get; set; } = y;

    public float Z { get; set; } = z;

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);

    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static Vec3 operator *(Vec3 a, float b) => new(a.X * b, a.Y * b, a.Z * b);

    public static Vec3 operator *(float a, Vec3 b) => b * a;

    public static Vec3 operator /(Vec3 a, float b) => new(a.X / b, a.Y / b, a.Z / b);

    public static Vec3 FromStream(Stream stream) {
        Vec3 output = default;

        using BinaryReader br = new(stream, Encoding.Unicode, true);
        output.X = br.ReadSingle();
        output.Y = br.ReadSingle();
        output.Z = br.ReadSingle();

        return output;
    }

    public static Vec3 Parse(string value) {
        var numbers = value.Trim('(', ')').Split(',').Select(f => float.Parse(f, CultureInfo.InvariantCulture)).ToArray();

        return new(numbers[0], numbers[1], numbers[2]);
    }

    public readonly void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(X);
        bw.Write(Y);
        bw.Write(Z);
    }

    public readonly bool Equals(Vec3 other) => Math.Abs(X - other.X) < 1e-4 && Math.Abs(Y - other.Y) < 1e-4 && Math.Abs(Z - other.Z) < 1e-4;

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{X},{Y},{Z}");
}

using System.Globalization;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal struct AssetHash(uint name, uint ns) {
    public static AssetHash Zero => new(0, 0);

    public uint Name {
        readonly get => name;
        set => Namespace = value;
    }

    public uint Namespace { get; set; } = ns;

    public static AssetHash FromStream(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        var name = br.ReadUInt32();
        var @namespace = br.ReadUInt32();

        return new(name, @namespace);
    }

    public static AssetHash Parse(string value)
        => new(uint.Parse(value[..8], NumberStyles.HexNumber, CultureInfo.InvariantCulture),
            uint.Parse(value.AsSpan(8, 8), NumberStyles.HexNumber, CultureInfo.InvariantCulture));

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(name);
        bw.Write(Namespace);
    }

    public readonly bool IsZero() => name == 0 && Namespace == 0;

    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{name:X8}{Namespace:X8}");
}

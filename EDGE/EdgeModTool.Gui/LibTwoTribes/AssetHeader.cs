using EdgeModTool.LibTwoTribes.Util;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class AssetHeader {
    private const int NameLength = 0x40;

    public AssetHeader(AssetUtil.EngineVersion engineVersion, string name, string nameSpace) {
        EngineVersion = engineVersion;
        Name = name;
        Namespace = nameSpace;
    }

    public AssetHeader(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);

        EngineVersion = (AssetUtil.EngineVersion)br.ReadUInt64();

        var name = new byte[NameLength];
        var @namespace = new byte[NameLength];

        _ = br.Read(name, 0, name.Length);
        _ = br.Read(@namespace, 0, @namespace.Length);

        Name = Encoding.ASCII.GetString(name).Replace("\0", string.Empty);
        Namespace = Encoding.ASCII.GetString(@namespace).Replace("\0", string.Empty);
    }

    public AssetUtil.EngineVersion EngineVersion { get; set; }

    public string Name { get; set; }

    public string Namespace { get; set; }

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        var name = BinaryUtil.PadOrTruncate(Encoding.ASCII.GetBytes(Name), NameLength);
        var @namespace = BinaryUtil.PadOrTruncate(Encoding.ASCII.GetBytes(Namespace), NameLength);

        bw.Write((ulong)EngineVersion);
        bw.Write(name);
        bw.Write(@namespace);
    }
}

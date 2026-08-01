using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class EANHeader {
    private AssetHash nodeChild;
    private AssetHash nodeSibling;

    public EANHeader() { }

    private EANHeader(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        Unknown1 = br.ReadSingle();
        Duration = br.ReadSingle();
        Zero1 = br.ReadUInt32();

        if (Zero1 != 0) {
            Console.WriteLine("ean_file_t::zero1 not 0!");
        }

        Zero2 = br.ReadUInt32();

        if (Zero2 != 0) {
            Console.WriteLine("ean_file_t::zero2 not 0!");
        }

        nodeChild = AssetHash.FromStream(stream);
        nodeSibling = AssetHash.FromStream(stream);
    }

    public float Unknown1 { get; set; }

    public float Duration { get; set; }

    public uint Zero1 { get; set; }

    public uint Zero2 { get; set; }

    public AssetHash NodeChild {
        get => nodeChild;
        set => nodeChild = value;
    }

    public AssetHash NodeSibling {
        get => nodeSibling;
        set => nodeSibling = value;
    }

    public static EANHeader FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(Unknown1);
        bw.Write(Duration);
        bw.Write(Zero1);
        bw.Write(Zero2);
        nodeChild.Save(stream);
        nodeSibling.Save(stream);
    }
}

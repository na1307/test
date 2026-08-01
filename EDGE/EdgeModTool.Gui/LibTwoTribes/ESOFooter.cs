using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ESOFooter {
    public ESOFooter() : this(0, 0, 0, 0) { }

    public ESOFooter(float v01, float v02, int v03, int v04) {
        V01 = v01;
        V02 = v02;
        V03 = v03;
        V04 = v04;
    }

    private ESOFooter(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);

        V01 = br.ReadSingle();
        V02 = br.ReadSingle();
        V03 = br.ReadInt32();
        V04 = br.ReadInt32();
    }

    public float V01 { get; set; }

    public float V02 { get; set; }

    public int V03 { get; set; }

    public int V04 { get; set; }

    public static ESOFooter FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(V01);
        bw.Write(V02);
        bw.Write(V03);
        bw.Write(V04);
    }
}

using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class EMADefaultTransform {
    public EMADefaultTransform() {
        ScaleU = 1;
        ScaleV = 1;
        Rotation = 0;
        TranslationU = 0;
        TranslationV = 0;
    }

    private EMADefaultTransform(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);

        ScaleU = br.ReadSingle();
        ScaleV = br.ReadSingle();
        Rotation = br.ReadSingle();
        TranslationU = br.ReadSingle();
        TranslationV = br.ReadSingle();
    }

    public float ScaleU { get; set; }

    public float ScaleV { get; set; }

    public float Rotation { get; set; }

    public float TranslationU { get; set; }

    public float TranslationV { get; set; }

    public static EMADefaultTransform FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(ScaleU);
        bw.Write(ScaleV);
        bw.Write(Rotation);
        bw.Write(TranslationU);
        bw.Write(TranslationV);
    }
}

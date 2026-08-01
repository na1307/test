using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class EMAAnimationBlock {
    public EMAAnimationBlock() {
        ProbablyTextureId = 0;
        ScaleU = new(1);
        ScaleV = new(1);
        Rotation = new(0);
        TranslationU = new(0);
        TranslationV = new(0);
    }

    private EMAAnimationBlock(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        ProbablyTextureId = br.ReadInt32();

        if (ProbablyTextureId != 0) {
            Console.WriteLine("ema_animation_block_t::unknown1 != 0");
        }

        ScaleU = new(stream);
        ScaleV = new(stream);
        Rotation = new(stream);
        TranslationU = new(stream);
        TranslationV = new(stream);
    }

    public int ProbablyTextureId { get; set; }

    public KeyframeBlock ScaleU { get; set; }

    public KeyframeBlock ScaleV { get; set; }

    public KeyframeBlock Rotation { get; set; }

    public KeyframeBlock TranslationU { get; set; }

    public KeyframeBlock TranslationV { get; set; }

    public static EMAAnimationBlock FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(ProbablyTextureId);
        ScaleU.Save(stream);
        ScaleV.Save(stream);
        Rotation.Save(stream);
        TranslationU.Save(stream);
        TranslationV.Save(stream);
    }
}

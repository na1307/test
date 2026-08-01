using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class KeyframeBlock {
    public KeyframeBlock() => Keyframes = [];

    public KeyframeBlock(float defaultValue) {
        DefaultValue = defaultValue;
        Keyframes = [];
    }

    public KeyframeBlock(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        var zero = br.ReadSingle();

        if (Math.Abs(zero) > 1e-5) {
            Console.WriteLine("keyframe_block_t::zero1 not 0!");
        }

        DefaultValue = br.ReadSingle();
        var numKeyframes = br.ReadInt32();
        Keyframes = new Keyframe[numKeyframes];

        for (var i = 0; i < numKeyframes; i++) {
            Keyframes[i] = Keyframe.FromStream(stream);
        }
    }

    public float DefaultValue { get; set; }

    public Keyframe[] Keyframes { get; set; }

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(0f); // 4 bytes pad.
        bw.Write(DefaultValue);
        bw.Write(Keyframes.Length);

        foreach (var keyframe in Keyframes) {
            keyframe.Save(stream);
        }
    }
}

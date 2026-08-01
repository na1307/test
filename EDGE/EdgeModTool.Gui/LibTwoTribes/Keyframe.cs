using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class Keyframe {
    public Keyframe(float time, float value, float velocity) {
        Time = time;
        Value = value;
        Velocity = velocity;
    }

    private Keyframe(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);

        Time = br.ReadSingle();
        Value = br.ReadSingle();
        Velocity = br.ReadSingle();
    }

    public float Time { get; set; }

    public float Value { get; set; }

    public float Velocity { get; set; }

    public static Keyframe FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(Time);
        bw.Write(Value);
        bw.Write(Velocity);
    }
}

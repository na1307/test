using System.Drawing;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class EMA : Asset {
    public EMA() { }

    private EMA(Stream stream) => CreateFromStream(stream);

    public string Name { get; set; } = null!;

    public EMATexture[] Textures { get; set; } = null!;

    public Color Color1 { get; set; }

    public Color Color2 { get; set; }

    public Color Color3 { get; set; }

    public Color Color4 { get; set; }

    public float Float1 { get; set; }

    public int Int1 { get; set; }

    public int Int2 { get; set; }

    public int Int3 { get; set; }

    public EMADefaultTransform[] DefaultTransforms { get; set; } = null!;

    public EMAAnimationBlock[] AnimationBlocks { get; set; } = null!;

    public int Footer4 { get; set; } = 4;

    public int Footer5 { get; set; } = 5;

    public static EMA FromFile(string path) {
        using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

        return FromStream(fs);
    }

    public static EMA FromStream(Stream stream) => new(stream);

    public void Save(string path) {
        using FileStream fs = new(path, FileMode.Create, FileAccess.Write);

        Save(fs);
    }

    public override void Save(Stream stream) {
        base.Save(stream);

        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write((short)(Name.Length + 1));
        bw.Write(Encoding.ASCII.GetBytes(Name));
        bw.Write((byte)0); // null terminator
        bw.Write(Textures.Length);

        foreach (var t in Textures) {
            t.Save(stream);
        }

        bw.Write(Color1.ToArgb());
        bw.Write(Color2.ToArgb());
        bw.Write(Color3.ToArgb());
        bw.Write(Color4.ToArgb());
        bw.Write(Float1);
        bw.Write(Int1);
        bw.Write(Int2);
        bw.Write(Int3);
        bw.Write(DefaultTransforms.Length);

        foreach (var t in DefaultTransforms) {
            t.Save(stream);
        }

        bw.Write(AnimationBlocks.Length);

        foreach (var t in AnimationBlocks) {
            t.Save(stream);
        }

        bw.Write(Footer4);
        bw.Write(Footer5);
    }

    protected override void CreateFromStream(Stream stream) {
        base.CreateFromStream(stream);

        using BinaryReader br = new(stream, Encoding.Unicode, true);
        var nameLen = br.ReadInt16();
        Name = Encoding.ASCII.GetString(br.ReadBytes(nameLen - 1));

        br.ReadByte(); // null terminator

        var numTextures = br.ReadInt32();
        Textures = new EMATexture[numTextures];

        for (var i = 0; i < numTextures; i++) {
            Textures[i] = EMATexture.FromStream(stream);
        }

        Color1 = Color.FromArgb(br.ReadInt32());
        Color2 = Color.FromArgb(br.ReadInt32());
        Color3 = Color.FromArgb(br.ReadInt32());
        Color4 = Color.FromArgb(br.ReadInt32());
        Float1 = br.ReadSingle();
        Int1 = br.ReadInt32();
        Int2 = br.ReadInt32();
        Int3 = br.ReadInt32();
        var numDefaultTransforms = br.ReadInt32(); // always(?) the same as the number of textures.

        if (numTextures != numDefaultTransforms) {
            Console.WriteLine("ema_file_t::num_textures != ema_file_t::num_default_transforms");
        }

        DefaultTransforms = new EMADefaultTransform[numDefaultTransforms];

        for (var i = 0; i < numDefaultTransforms; i++) {
            DefaultTransforms[i] = EMADefaultTransform.FromStream(stream);
        }

        var numAnimationBlocks = br.ReadInt32();
        AnimationBlocks = new EMAAnimationBlock[numAnimationBlocks];

        for (var i = 0; i < numAnimationBlocks; i++) {
            AnimationBlocks[i] = EMAAnimationBlock.FromStream(stream);
        }

        Footer4 = br.ReadInt32();

        if (Footer4 != 4) {
            Console.WriteLine("ema_file_t::unknown5 != 4");
        }

        Footer5 = br.ReadInt32();

        if (Footer5 != 5) {
            Console.WriteLine("ema_file_t::unknown6 != 5");
        }
    }
}

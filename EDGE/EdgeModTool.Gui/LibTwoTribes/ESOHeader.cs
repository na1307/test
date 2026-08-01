using EdgeModTool.LibTwoTribes.Util;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ESOHeader {
    private Vec3 boundingMax;
    private Vec3 boundingMin;
    private AssetHash nodeChild;
    private AssetHash nodeSibling;
    private Vec3 rotate;
    private Vec3 scale;
    private Vec3 translate;

    public ESOHeader() { }

    private ESOHeader(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        V01 = br.ReadInt32();
        V02 = br.ReadInt32();
        nodeChild = AssetHash.FromStream(stream);
        nodeSibling = AssetHash.FromStream(stream);
        V07 = br.ReadInt32();
        V08 = br.ReadInt32();
        V09 = br.ReadInt32();
        ScaleXYZ = br.ReadSingle();
        translate = Vec3.FromStream(stream);
        rotate = Vec3.FromStream(stream);
        scale = Vec3.FromStream(stream);
        V20 = br.ReadSingle();
        V21 = br.ReadInt32();
        NumModels = br.ReadInt32();

        if (NumModels > 0) {
            boundingMin = Vec3.FromStream(stream);
            boundingMax = Vec3.FromStream(stream);
        } else {
            boundingMin = default;
            boundingMax = default;
        }
    }

    public int V01 { get; set; }

    public int V02 { get; set; }

    public AssetHash NodeChild {
        get => nodeChild;
        set => nodeChild = value;
    }

    public AssetHash NodeSibling {
        get => nodeSibling;
        set => nodeSibling = value;
    }

    public int V07 { get; set; }

    public int V08 { get; set; }

    public int V09 { get; set; }

    public float ScaleXYZ { get; set; }

    public Vec3 Translate {
        get => translate;
        set => translate = value;
    }

    public Vec3 Rotate {
        get => rotate;
        set => rotate = value;
    }

    public Vec3 Scale {
        get => scale;
        set => scale = value;
    }

    public float V20 { get; set; }

    public int V21 { get; set; }

    public int NumModels { get; set; }

    public Vec3 BoundingMin {
        get => boundingMin;
        set => boundingMin = value;
    }

    public Vec3 BoundingMax {
        get => boundingMax;
        set => boundingMax = value;
    }

    public static ESOHeader FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(V01);
        bw.Write(V02);
        nodeChild.Save(stream);
        nodeSibling.Save(stream);
        bw.Write(V07);
        bw.Write(V08);
        bw.Write(V09);
        bw.Write(ScaleXYZ);
        translate.Save(stream);
        rotate.Save(stream);
        scale.Save(stream);
        bw.Write(V20);
        bw.Write(V21);
        bw.Write(NumModels);

        if (NumModels > 0) {
            boundingMin.Save(stream);
            boundingMax.Save(stream);
        }
    }
}

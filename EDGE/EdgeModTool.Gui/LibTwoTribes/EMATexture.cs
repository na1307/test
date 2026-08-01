using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class EMATexture {
    private const int IntArray1Length = 4;
    private const int IntArray2Length = 3;
    private AssetHash asset;
    private int[] intArray1;
    private int[] intArray2;

    public EMATexture() {
        asset = AssetHash.Zero;
        intArray1 = new int[IntArray1Length];
        intArray2 = new int[IntArray2Length];
    }

    private EMATexture(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        asset = AssetHash.FromStream(stream);
        intArray1 = new int[IntArray1Length];

        for (var i = 0; i < IntArray1Length; i++) {
            intArray1[i] = br.ReadInt32();
        }

        intArray2 = new int[IntArray2Length];

        for (var i = 0; i < IntArray2Length; i++) {
            intArray2[i] = br.ReadInt32();
        }
    }

    public AssetHash Asset {
        get => asset;
        set => asset = value;
    }

    public int[] IntArray1 {
        get => intArray1;
        set {
            if (value.Length != IntArray1Length) {
                throw new("IntArray1 must have a length of " + IntArray1Length);
            }

            intArray1 = value;
        }
    }

    public int[] IntArray2 {
        get => intArray2;
        set {
            if (value.Length != IntArray2Length) {
                throw new("IntArray2 must have a length of " + IntArray2Length);
            }

            intArray2 = value;
        }
    }

    public static EMATexture FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        asset.Save(stream);

        for (var i = 0; i < IntArray1Length; i++) {
            bw.Write(intArray1[i]);
        }

        for (var i = 0; i < IntArray2Length; i++) {
            bw.Write(intArray2[i]);
        }
    }
}

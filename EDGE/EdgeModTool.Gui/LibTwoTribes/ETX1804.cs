using EdgeModTool.LibTwoTribes.Util;
using SkiaSharp;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ETX1804 : ETX {
    private SKBitmap bitmap = null!;

    internal ETX1804(string path) {
        using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

        CreateFromStream(fs);
    }

    internal ETX1804(Stream stream) => CreateFromStream(stream);

    private ETX1804() { }

    public static ETX1804 CreateFromImage(string imagePath, string? name = null, string nameSpace = "textures")
        => CreateFromImage(SKBitmap.Decode(imagePath), name ?? Path.GetFileNameWithoutExtension(imagePath), nameSpace);

    public static ETX1804 CreateFromImage(SKBitmap bitmap, string name, string nameSpace = "textures")
        => CreateFromImage(bitmap, new(AssetUtil.EngineVersion.Version1804Edge, name, nameSpace));

    public static ETX1804 CreateFromImage(SKBitmap bitmap, AssetHeader header)
        => new() {
            bitmap = bitmap,
            AssetHeader = header
        };

    public override SKBitmap GetBitmap() => bitmap;

    public override void Save(string path) {
        using FileStream fsOut = new(path, FileMode.Create, FileAccess.Write);

        Save(fsOut);
    }

    public override void Save(Stream stream) {
        base.Save(stream);

        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        bw.Write(bitmap.Width);
        bw.Write(bitmap.Height);
        bw.Write(2);
        var data = Serialize(bitmap);
        bw.Write(data.Length);
        bw.Write(data);
    }

    protected override void CreateFromStream(Stream stream) {
        base.CreateFromStream(stream);

        using BinaryReader br = new(stream, Encoding.Unicode, true);

        var width = br.ReadInt16();
        var height = br.ReadInt16();
        var unknown2 = br.ReadInt32();

        if (unknown2 != 2) {
            throw new NotSupportedException();
        }

        var dataLength = br.ReadInt32();
        var data = br.ReadBytes(dataLength);

        bitmap = RenderBGRA8888(data, width, height, 1);
    }
}

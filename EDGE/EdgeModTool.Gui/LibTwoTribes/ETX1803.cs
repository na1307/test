using EdgeModTool.LibTwoTribes.Util;
using SkiaSharp;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ETX1803 : ETX {
    private SKBitmap bitmap = null!;

    internal ETX1803(Stream stream) => CreateFromStream(stream);

    private ETX1803() { }

    public static ETX1803 CreateFromImage(string imagePath, string? name = null, string nameSpace = "textures")
        => CreateFromImage(SKBitmap.Decode(imagePath), name ?? Path.GetFileNameWithoutExtension(imagePath), nameSpace);

    public static ETX1803 CreateFromImage(SKBitmap bitmap, string name, string nameSpace = "textures")
        => CreateFromImage(bitmap, new(AssetUtil.EngineVersion.Version1803Rush, name, nameSpace));

    public static ETX1803 CreateFromImage(SKBitmap bitmap, AssetHeader header)
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

        bitmap.Encode(stream, SKEncodedImageFormat.Png, 100);
    }

    protected override void CreateFromStream(Stream stream) {
        base.CreateFromStream(stream);

        using BinaryReader br = new(stream, Encoding.Unicode, true);
        var buffer = br.ReadBytes((int)(stream.Length - stream.Position));
        using MemoryStream ms = new(buffer);

        ms.Position = 0;
        bitmap = SKBitmap.Decode(ms);
    }
}

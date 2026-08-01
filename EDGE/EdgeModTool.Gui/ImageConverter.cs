using EdgeModTool.Level;
using SkiaSharp;
using System.Drawing;

namespace EdgeModTool;

internal static class ImageConverter {
    public static unsafe void Save(Color[] pixels, int width, int height, string path) {
        using SKBitmap bitmap = new(width, height, SKColorType.Argb4444, SKAlphaType.Unknown);
        var pointer = (int*)bitmap.GetPixels();

        for (var i = 0; i < pixels.Length; i++) {
            *pointer++ = pixels[i].ToArgb();
        }

        using var file = File.Create(path);

        bitmap.Encode(file, SKEncodedImageFormat.Png, 100);
    }

    public static unsafe Color[] Load(string path, out Size2D size) {
        SKBitmap? clone = null;

        try {
            using (var org = SKBitmap.Decode(path)) // convert the pixel format
            {
                size = new((ushort)org.Width, (ushort)org.Height);
                clone = new(org.Width, org.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);

                using SKCanvas g = new(clone);

                g.DrawBitmap(org, new SKRect(0, 0, clone.Width, clone.Height));
            }

            var result = new Color[clone.Width * clone.Height];
            var pointer = (int*)clone.GetPixels();

            for (var i = 0; i < result.Length; i++) {
                result[i] = Color.FromArgb(*pointer++);
            }

            return result;
        } finally {
            clone?.Dispose();
        }
    }
}

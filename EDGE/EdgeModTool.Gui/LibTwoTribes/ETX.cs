using EdgeModTool.LibTwoTribes.Util;
using SkiaSharp;

/*
 *
 * The ETX format has shown up in 3 Two Tribes games, to my knowledge. (RUSH, EDGE, and Toki Tori 2)
 * All versions are prefixed by the Two Tribes 136-byte Asset header, followed by the actual data payload.
 *
 * RUSH (engine version 18/03):
 * The latest build available on Steam has the payload consist of a standard PNG image.
 * Simply read from the end of the Asset Header to the end of the file.
 *
 * EDGE (engine version 18/04):
 * Older builds used the same format as RUSH.
 * Newer builds have the format consist of an ETX Header, followed by the image data in BGRA8888 format. `Header.Format` contains the data length in bytes.
 *
 * Toki Tori 2 (engine version 00/04):
 * This is a fun one.
 * After the ETX Header are two 32-bit integers, representing compressed and decompressed data lengths.
 * Following them is the image data, compressed under the FastLZ algorithm.
 * Image format can either be BGRA8888 or A8, depending on the value in Header.Format.
 *
 * The extension also makes an appearance for two files in Toki Tori 1 (Steam version), but the content is simply a DDS file without a TT Asset header.
 *
 */

namespace EdgeModTool.LibTwoTribes;

internal abstract class ETX : Asset {
    public static ETX FromFile(string path) {
        using FileStream fsIn = new(path, FileMode.Open, FileAccess.Read);

        return FromStream(fsIn);
    }

    public static ETX FromStream(Stream stream) {
        var pos = stream.Position;
        AssetHeader assetHeader = new(stream);
        stream.Position = pos;

        if (assetHeader.EngineVersion == AssetUtil.EngineVersion.Version1804Edge) {
            return new ETX1804(stream);
        }

        if (((long)assetHeader.EngineVersion & 0xFF) == 0x03) {
            return new ETX1803(stream);
        }

        throw new InvalidDataException("Unrecognised engine version " + AssetUtil.GetEngineVersionName(assetHeader.EngineVersion));
    }

    public abstract SKBitmap GetBitmap();

    public abstract void Save(string path);

    protected static unsafe byte[] Serialize(SKBitmap bmp) {
        var buffer = new byte[bmp.Width * bmp.Height * 4];
        var ptrBmp = (byte*)bmp.GetPixels();

        for (var i = 0; i < bmp.Width * bmp.Height * 4; i++) {
            buffer[i] = ptrBmp[i];
        }

        return buffer;
    }

    protected static unsafe SKBitmap RenderBGRA8888(byte[] textureData, int width, int height, int depth) {
        SKBitmap bmp = new(width * depth, height);
        var ptrBmp = (byte*)bmp.GetPixels();

        // the raw data is BGRA, and ptrBmp takes BGRA, so we can copy the data straight across.
        for (var ix = 0; ix < width; ix++) {
            for (var iy = 0; iy < height; iy++) {
                for (var iz = 0; iz < depth; iz++) {
                    var x = ix + width * iz;
                    var y = iy;

                    var wIndex = ix + iy * width + iz * width * height;
                    var sIndex = y * bmp.RowBytes + x * 4;

                    for (var j = 0; j < 4; j++) {
                        ptrBmp[sIndex + j] = textureData[wIndex * 4 + j]; // BGRA
                    }
                }
            }
        }

        return bmp;
    }
}

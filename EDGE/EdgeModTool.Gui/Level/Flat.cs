using System.Collections;
using System.Drawing;

namespace EdgeModTool.Level;

internal sealed class Flat(int width, int length, byte[]? array = null) : IXSerializable {
    public static readonly Color Empty = Color.Black;
    public static readonly Color HalfBlock = Color.FromArgb(unchecked((int)0xFFFFFF80));
    public static readonly Color Block = Color.White;
    public static readonly Color HalfBlockModelOnly = Color.FromArgb(unchecked((int)0xFF00FF80));
    public static readonly Color BlockModelOnly = Color.Cyan;
    public readonly int Width = width;
    public readonly int Length = length;
    private readonly BitArray data = array is null ? new(GetBytes(width, length) << 3) : new BitArray(array);
    private Color[]? detailedInformation;

    public Flat(Size2D size, byte[]? array = null) : this(size.Width, size.Length, array) { }

    public Flat(BinaryReader reader, Size2D size) : this(size.Width, size.Length, reader.ReadBytes(size.FlatBytes)) { }

    public Flat(string path, Size2D size) : this(size.Width, size.Length) {
        if (!File.Exists(path)) {
            return;
        }

        var array2 = ImageConverter.Load(path, out var fileSize);

        if (fileSize == size) {
            detailedInformation = array2;
            var pos = 0;

            for (var y = 0; y < size.Length; y++) {
                for (var x = 0; x < size.Width; x++, pos++) {
                    var color = array2[pos];
                    this[x, y] = color.R == 255;
                }
            }
        }
    }

    public int Bytes => GetBytes(Width, Length);

    public bool this[int x, int y] {
        get => data[GetPosition(x, y)];
        set => data[GetPosition(x, y)] = value;
    }

    public static int GetBytes(int width, int length) {
        return (width * length + 7) >> 3;
    }

    public void InitImage(bool half = false) {
        detailedInformation = new Color[Width * Length];
        var block = half ? HalfBlock : Block;

        for (var y = 0; y < Length; ++y) {
            for (var x = 0; x < Width; ++x) {
                detailedInformation[y * Width + x] = this[x, y] ? block : Empty;
            }
        }
    }

    public void SaveToImage(string path, bool half = false) {
        if (detailedInformation is null) {
            InitImage(half);
        }

        for (var y = 0; y < Length; y++) {
            for (var x = 0; x < Width; x++) {
                if (GetColor(x, y) != Empty) {
                    ImageConverter.Save(detailedInformation!, Width, Length, path);

                    return;
                }
            }
        }
    }

    public Color GetColor(int x, int y, bool half = false) {
        if (detailedInformation is null) {
            InitImage(half);
        }

        return detailedInformation![y * Width + x];
    }

    public void SetColor(int x, int y, Color color, bool half = false) {
        if (detailedInformation is null) {
            InitImage(half);
        }

        detailedInformation![y * Width + x] = color;
    }

    public void Write(BinaryWriter writer) {
        var array2 = new byte[Bytes];

        data.CopyTo(array2, 0);
        writer.Write(array2);
    }

    private int GetPosition(int x, int y) {
        var pos = y * Width + x;
        var posBit = pos & 7; // posBase = pos & ~7 = pos - posBit

        return pos + 7 - posBit - posBit; // return posBase + (7 - posBit);
    }
}

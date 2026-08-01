using EdgeModTool.LibTwoTribes.Util;
using System.Drawing;
using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ESOModel {
    private AssetHash materialAsset;

    /// <summary>
    /// Initialize ESOModel with the number of vertices.
    /// </summary>
    /// <param name="typeFlags">Initial value for type flags.</param>
    /// <param name="numVerts">Number of vertices there will be. This is only used for reserving storage. You can
    /// have other number of vertices too if you'd like to.</param>
    public ESOModel(Flags typeFlags = 0, int numVerts = 0) => Initialize(typeFlags, numVerts);

    private ESOModel(Stream stream) {
        using BinaryReader br = new(stream, Encoding.Unicode, true);
        materialAsset = AssetHash.FromStream(stream);
        TypeFlags = (Flags)br.ReadUInt32();
        var numVerts = br.ReadInt32();

        Initialize(TypeFlags, numVerts);

        if (br.ReadInt32() * 3 != numVerts) {
            Console.WriteLine("Polygon count is not correct! It will be ignored.");
        }

        var unknown = br.ReadInt32(); // not a clue. seems to be always zero.

        if (unknown != 0) {
            Console.WriteLine("eso_model_t::unknown1 != 0");
        }

        for (var i = 0; i < numVerts; i++) {
            Vertices.Add(Vec3.FromStream(stream));
        }

        if (HasNormals) {
            for (var i = 0; i < numVerts; i++) {
                Normals.Add(Vec3.FromStream(stream));
            }
        }

        if (HasColors) {
            for (var i = 0; i < numVerts; i++) {
                Colors.Add(Color.FromArgb(br.ReadInt32()));
            }
        }

        if (HasTexCoords) {
            for (var i = 0; i < numVerts; i++) {
                TexCoords.Add(Vec2.FromStream(stream));
            }
        }

        if (HasTexCoords2) {
            for (var i = 0; i < numVerts; i++) {
                TexCoords2.Add(Vec2.FromStream(stream));
            }
        }

        for (var i = 0; i < numVerts; i++) {
            if (br.ReadUInt16() != i) {
                Console.WriteLine("eso_model_t::indices not supported!");
            }
        }
    }

    [Flags]
    internal enum Flags : uint {
        Normals = 0x00000001,
        Colors = 0x00000002,
        TexCoords = 0x00000004,
        TexCoords2 = 0x00000008 // only seen in RUSH for the checkerboard pattern
    }

    public AssetHash MaterialAsset {
        get => materialAsset;
        set => materialAsset = value;
    }

    public Flags TypeFlags { get; set; }

    public List<Vec3> Vertices { get; set; } = null!;

    public List<Vec3> Normals { get; set; } = null!;

    public List<Color> Colors { get; set; } = null!;

    public List<Vec2> TexCoords { get; set; } = null!;

    public List<Vec2> TexCoords2 { get; set; } = null!;

    public bool HasNormals {
        get => TypeFlags.HasFlag(Flags.Normals);
        set {
            if (value) {
                TypeFlags |= Flags.Normals;
            } else {
                TypeFlags &= ~Flags.Normals;
            }
        }
    }

    public bool HasColors {
        get => TypeFlags.HasFlag(Flags.Colors);
        set {
            if (value) {
                TypeFlags |= Flags.Colors;
            } else {
                TypeFlags &= ~Flags.Colors;
            }
        }
    }

    public bool HasTexCoords {
        get => TypeFlags.HasFlag(Flags.TexCoords);
        set {
            if (value) {
                TypeFlags |= Flags.TexCoords;
            } else {
                TypeFlags &= ~Flags.TexCoords;
            }
        }
    }

    public bool HasTexCoords2 {
        get => TypeFlags.HasFlag(Flags.TexCoords2);
        set {
            if (value) {
                TypeFlags |= Flags.TexCoords2;
            } else {
                TypeFlags &= ~Flags.TexCoords2;
            }
        }
    }

    public static ESOModel FromStream(Stream stream) => new(stream);

    public void Save(Stream stream) {
        using BinaryWriter bw = new(stream, Encoding.Unicode, true);

        materialAsset.Save(stream);

        bw.Write((uint)TypeFlags);
        bw.Write(Vertices.Count);
        bw.Write(Vertices.Count / 3);
        bw.Write(0);

        for (var i = 0; i < Vertices.Count; i++) {
            Vertices[i].Save(stream);
        }

        if (TypeFlags.HasFlag(Flags.Normals)) {
            for (var i = 0; i < Vertices.Count; i++) {
                Normals[i].Save(stream);
            }
        }

        if (TypeFlags.HasFlag(Flags.Colors)) {
            for (var i = 0; i < Vertices.Count; i++) {
                bw.Write(Colors[i].ToArgb());
            }
        }

        if (TypeFlags.HasFlag(Flags.TexCoords)) {
            for (var i = 0; i < Vertices.Count; i++) {
                TexCoords[i].Save(stream);
            }
        }

        if (TypeFlags.HasFlag(Flags.TexCoords2)) {
            for (var i = 0; i < Vertices.Count; i++) {
                TexCoords2[i].Save(stream);
            }
        }

        for (ushort i = 0; i < Vertices.Count; i++) {
            bw.Write(i);
        }
    }

    private void Initialize(Flags typeFlags, int numVerts) {
        TypeFlags = typeFlags;
        Vertices = new(numVerts);
        Colors = new(HasColors ? numVerts : 0);
        Normals = new(HasNormals ? numVerts : 0);
        TexCoords = new(HasTexCoords ? numVerts : 0);
        TexCoords2 = new(HasTexCoords2 ? numVerts : 0);
    }
}

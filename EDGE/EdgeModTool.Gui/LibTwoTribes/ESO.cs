using System.Text;

namespace EdgeModTool.LibTwoTribes;

internal sealed class ESO : Asset {
    public ESO() { }

    private ESO(Stream stream) => CreateFromStream(stream);

    public ESOHeader Header { get; set; } = null!;

    public ESOModel[] Models { get; set; } = null!;

    public bool HasFooter { get; set; }

    public ESOFooter Footer { get; set; } = null!;

    public static ESO FromFile(string path) {
        using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

        return FromStream(fs);
    }

    public static ESO FromStream(Stream stream) => new(stream);

    public void Save(string path) {
        using FileStream fs = new(path, FileMode.Create, FileAccess.Write);

        Save(fs);
    }

    public override void Save(Stream stream) {
        base.Save(stream);
        Header.Save(stream);

        foreach (var t in Models) {
            t.Save(stream);
        }

        if (Models.Length > 0) {
            using BinaryWriter bw = new(stream, Encoding.Unicode, true);

            if (HasFooter) {
                bw.Write(1);
                Footer.Save(stream);
            } else {
                bw.Write(0);
            }
        }
    }

    protected override void CreateFromStream(Stream stream) {
        base.CreateFromStream(stream);

        Header = ESOHeader.FromStream(stream);
        Models = new ESOModel[Header.NumModels];

        for (var i = 0; i < Header.NumModels; i++) {
            Models[i] = ESOModel.FromStream(stream);
        }

        using BinaryReader br = new(stream, Encoding.Unicode, true);

        if (Header.NumModels > 0) {
            var i = br.ReadUInt32();

            if (i > 1) {
                Console.WriteLine("eso_file_t::footer_check is not a valid bool.");
            }

            Footer = HasFooter == (i == 1) ? ESOFooter.FromStream(stream) : new();
        } else {
            HasFooter = false;
            Footer = new();
        }
    }
}

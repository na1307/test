namespace EdgeModTool.LibTwoTribes;

internal sealed class EAN : Asset {
    public EAN() { }

    private EAN(Stream stream) => CreateFromStream(stream);

    public EANHeader Header { get; set; } = null!;

    public KeyframeBlock BlockTranslateX { get; set; } = null!;

    public KeyframeBlock BlockTranslateY { get; set; } = null!;

    public KeyframeBlock BlockTranslateZ { get; set; } = null!;

    public KeyframeBlock BlockRotateX { get; set; } = null!;

    public KeyframeBlock BlockRotateY { get; set; } = null!;

    public KeyframeBlock BlockRotateZ { get; set; } = null!;

    public KeyframeBlock BlockScaleX { get; set; } = null!;

    public KeyframeBlock BlockScaleY { get; set; } = null!;

    public KeyframeBlock BlockScaleZ { get; set; } = null!;

    public static EAN FromFile(string path) {
        using FileStream fs = new(path, FileMode.Open, FileAccess.Read);

        return FromStream(fs);
    }

    public static EAN FromStream(Stream stream) => new(stream);

    public void Save(string path) {
        using FileStream fs = new(path, FileMode.Create, FileAccess.Write);

        Save(fs);
    }

    public override void Save(Stream stream) {
        base.Save(stream);
        Header.Save(stream);
        BlockTranslateX.Save(stream);
        BlockTranslateY.Save(stream);
        BlockTranslateZ.Save(stream);
        BlockRotateX.Save(stream);
        BlockRotateY.Save(stream);
        BlockRotateZ.Save(stream);
        BlockScaleX.Save(stream);
        BlockScaleY.Save(stream);
        BlockScaleZ.Save(stream);
    }

    protected override void CreateFromStream(Stream stream) {
        base.CreateFromStream(stream);

        Header = EANHeader.FromStream(stream);
        BlockTranslateX = new(stream);
        BlockTranslateY = new(stream);
        BlockTranslateZ = new(stream);
        BlockRotateX = new(stream);
        BlockRotateY = new(stream);
        BlockRotateZ = new(stream);
        BlockScaleX = new(stream);
        BlockScaleY = new(stream);
        BlockScaleZ = new(stream);
    }
}

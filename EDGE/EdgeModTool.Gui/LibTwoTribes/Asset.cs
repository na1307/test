namespace EdgeModTool.LibTwoTribes;

internal abstract class Asset {
    public AssetHeader AssetHeader { get; set; } = null!;

    public virtual void Save(Stream stream) => AssetHeader.Save(stream);

    protected virtual void CreateFromStream(Stream stream) => AssetHeader = new(stream);
}

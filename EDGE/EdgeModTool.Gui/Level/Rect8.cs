namespace EdgeModTool.Level;

internal struct Rect8(Point2D8 point, Point2D8 size) {
    public Point2D8 Point = point;
    public Point2D8 Size = size;

    public Rect8(int x, int y, int sizeX, int sizeY) : this(new((byte)x, (byte)y), new((byte)sizeX, (byte)sizeY)) { }

    public Rect8(BinaryReader reader) : this(new(reader), new(reader)) { }

    public readonly void Write(BinaryWriter writer) {
        Point.Write(writer);
        Size.Write(writer);
    }
}

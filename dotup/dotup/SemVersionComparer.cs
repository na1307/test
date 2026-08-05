namespace Dotup;

internal sealed class SemVersionComparer : IComparer<SemVersion> {
    public static readonly SemVersionComparer Default = new();

    public int Compare(SemVersion? x, SemVersion? y) {
        if (ReferenceEquals(x, y)) {
            return 0;
        }

        if (x is null) {
            return -1;
        }

        if (y is null) {
            return 1;
        }

        return x.ComparePrecedenceTo(y);
    }
}

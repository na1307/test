namespace SimpleHashTab;

internal enum TYMED : uint {
    NULL = 0,
    HGLOBAL = 1,
    FILE = 2,
    ISTREAM = 4,
    ISTORAGE = 8,
    GDI = 16,
    MFPICT = 32,
    ENHMF = 64
}

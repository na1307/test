using System.Runtime.InteropServices;

namespace Etude.Alpm;

[StructLayout(LayoutKind.Explicit)]
public struct PkgOriginData {
    [FieldOffset(0)]
    public IntPtr db;

    [FieldOffset(0)]
    public IntPtr file;
}

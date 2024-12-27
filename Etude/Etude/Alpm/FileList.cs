using System.Runtime.InteropServices;

namespace Etude.Alpm;

[StructLayout(LayoutKind.Sequential)]
public struct FileList {
    public nuint count;
    public IntPtr files;
}

using System.Runtime.InteropServices;

namespace Etude.Alpm;

[StructLayout(LayoutKind.Sequential)]
public struct AlpmList {
    public IntPtr data;
    public IntPtr prev;
    public IntPtr next;
}

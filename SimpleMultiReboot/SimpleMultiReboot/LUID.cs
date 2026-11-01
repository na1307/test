using System.Runtime.InteropServices;

namespace SimpleMultiReboot;

[StructLayout(LayoutKind.Sequential)]
internal struct LUID {
    public uint LowPart;
    public int HighPart;
}

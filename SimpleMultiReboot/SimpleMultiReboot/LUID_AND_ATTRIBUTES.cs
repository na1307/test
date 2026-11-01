using System.Runtime.InteropServices;

namespace SimpleMultiReboot;

[StructLayout(LayoutKind.Sequential)]
internal struct LUID_AND_ATTRIBUTES {
    public LUID Luid;
    public uint Attributes;
}

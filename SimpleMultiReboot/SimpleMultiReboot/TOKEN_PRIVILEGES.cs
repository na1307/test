using System.Runtime.InteropServices;

namespace SimpleMultiReboot;

[StructLayout(LayoutKind.Sequential)]
internal struct TOKEN_PRIVILEGES {
    public uint PrivilegeCount;
    public LUID_AND_ATTRIBUTES Privileges;
}

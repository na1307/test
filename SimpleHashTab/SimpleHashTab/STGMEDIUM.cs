namespace SimpleHashTab;

[StructLayout(LayoutKind.Sequential)]
internal struct STGMEDIUM {
    public TYMED tymed;
    public IntPtr data;
    public IntPtr pUnkForRelease;
}

namespace SimpleHashTab;

[StructLayout(LayoutKind.Sequential)]
internal struct FORMATETC {
    public CLIPFORMAT cfFormat;
    public IntPtr ptd;
    public DVASPECT dwAspect;
    public int lindex;
    public TYMED tymed;
}

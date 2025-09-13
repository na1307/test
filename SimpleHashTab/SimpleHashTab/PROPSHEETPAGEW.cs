namespace SimpleHashTab;

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct PROPSHEETPAGEW {
    public readonly uint dwSize = (uint)sizeof(PROPSHEETPAGEW);
    public PSPFLAGS dwFlags;
    public IntPtr hInstance;
    public IntPtr pszTemplate;
    public IntPtr hIcon;
    public IntPtr pszTitle;
    public IntPtr pfnDlgProc;
    public IntPtr lParam;
    public IntPtr pfnCallback;
    public uint* pcRefParent;
    public IntPtr pszHeaderTitle;
    public IntPtr pszHeaderSubTitle;
    public IntPtr hActCtx;
    public IntPtr hbmHeader;

    public PROPSHEETPAGEW() { }
}

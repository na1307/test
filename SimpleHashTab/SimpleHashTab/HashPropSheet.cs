using System.Text;

namespace SimpleHashTab;

[GeneratedComClass]
[Guid("1C4DAF34-4D19-4A75-B31C-E915FF962CF4")]
internal sealed partial class HashPropSheet : IShellExtInit, IShellPropSheetExt {
    private string? fs;
    private IntPtr hcHwnd;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern uint DragQueryFileW(IntPtr hDrop, uint iFile, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder? lpszFile, uint cch);

    public void Initialize(IntPtr pidlFolder, IDataObject? pdtobj, IntPtr hkeyProgID) {
        ArgumentNullException.ThrowIfNull(pdtobj);

        FORMATETC f = new() {
            cfFormat = CLIPFORMAT.CF_HDROP,
            ptd = IntPtr.Zero,
            dwAspect = DVASPECT.CONTENT,
            lindex = -1,
            tymed = TYMED.HGLOBAL
        };

        var stg = pdtobj.GetData(in f);
        var data = stg.data;

        if (DragQueryFileW(data, uint.MaxValue, null, 0) > 1) {
            throw new("More than one file");
        }

        StringBuilder buf = new(260);
        var result = DragQueryFileW(data, 0, buf, 260);

        if (result == 0) {
            throw new($"DragQueryFileW Failed: {result}");
        }

        fs = buf.ToString();

        ReleaseStgMedium(in stg);
    }

    public void AddPages(LPFNADDPROPSHEETPAGE pfnAddPage, IntPtr lParam) {
        PROPSHEETPAGEW psp = new() {
            dwFlags = PSPFLAGS.USETITLE,
            hInstance = GetBridgeInstance(),
            pszTemplate = GetResource(),
            pszTitle = Marshal.StringToHGlobalUni("해시"),
            pfnDlgProc = Marshal.GetFunctionPointerForDelegate(new DLGPROC(DlgProc))
        };

        var hPsp = CreatePropertySheetPageW(in psp);

        if (hPsp != IntPtr.Zero && !pfnAddPage(hPsp, lParam)) {
            DestroyPropertySheetPage(hPsp);
        }
    }

    public void ReplacePage(uint uPageID, LPFNADDPROPSHEETPAGE pfnReplaceWith, IntPtr lParam) => throw new NotImplementedException();

    [DllImport("ole32.dll", ExactSpelling = true)]
    private static extern void ReleaseStgMedium(in STGMEDIUM unnamedParam1);

    [DllImport("nativebridge.dll", ExactSpelling = true)]
    private static extern IntPtr GetBridgeInstance();

    [DllImport("nativebridge.dll", ExactSpelling = true)]
    private static extern IntPtr GetResource();

    [DllImport("comctl32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern IntPtr CreatePropertySheetPageW(in PROPSHEETPAGEW constPropSheetPagePointer);

    [DllImport("comctl32.dll", ExactSpelling = true)]
    private static extern bool DestroyPropertySheetPage(IntPtr unnamedParam1);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool PostMessageW(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    private IntPtr DlgProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam) {
        switch (uMsg) {
            case WindowsMessages.WM_INITDIALOG:
                HashControl hc = new(fs!);
                hcHwnd = hc.Handle;

                SetParent(hc.Handle, hWnd);

                return 1;

            case WindowsMessages.WM_ERASEBKGND:
                return 1;

            case WindowsMessages.WM_DROPFILES:
                PostMessageW(hcHwnd, uMsg, wParam, lParam);

                break;
        }

        return 0;
    }
}

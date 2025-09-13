namespace SimpleHashTab;

[GeneratedComInterface]
[Guid("000214E9-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal partial interface IShellPropSheetExt {
    void AddPages(LPFNADDPROPSHEETPAGE pfnAddPage, IntPtr lParam);

    void ReplacePage(uint uPageID, LPFNADDPROPSHEETPAGE pfnReplaceWith, IntPtr lParam);
}

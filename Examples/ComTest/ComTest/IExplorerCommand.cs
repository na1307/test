using Bluehill.NativeCom;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ComTest;

public enum Sigdn {
    Normaldisplay = 0,
    Parentrelativeparsing = unchecked((int)0x80018001),
    Desktopabsoluteparsing = unchecked((int)0x80028000),
    Parentrelativeediting = unchecked((int)0x80031001),
    Desktopabsoluteediting = unchecked((int)0x8004c000),
    Filesyspath = unchecked((int)0x80058000),
    Url = unchecked((int)0x80068000),
    Parentrelativeforaddressbar = unchecked((int)0x8007c001),
    Parentrelative = unchecked((int)0x80080001),
    Parentrelativeforui = unchecked((int)0x80094001)
}

[GeneratedComInterface]
[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public unsafe partial interface IShellItem {
    void* BindToHandler(void* bc, in Guid bhid, in Guid iid);

    IShellItem? GetParent();

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string GetDisplayName(Sigdn sigdnName);

    [PreserveSig]
    int GetAttributes(uint sfgaoMask, out uint sfgaoAttribs);

    [PreserveSig]
    int Compare(IShellItem si, uint hint, out int order);
}

[GeneratedComInterface]
[Guid("b63ea76d-1f85-456f-a19c-48159efa858b")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public unsafe partial interface IShellItemArray {
    void* BindToHandler(void* bc, in Guid bhid, in Guid iid);

    void* GetPropertyStore(int flags, in Guid iid);

    void* GetPropertyDescriptionList(void* keyType, in Guid iid);

    [PreserveSig]
    int GetAttributes(int attribFlags, uint sfgaoMask, out uint sfgaoAttribs);

    uint GetCount();

    IShellItem? GetItemAt(uint index);

    void* EnumItems();
}

[Flags]
public enum Expcmdstate : uint {
    Enabled = 0,
    Disabled = 0x1,
    Hidden = 0x2,
    Checkbox = 0x4,
    Checked = 0x8,
    Radiocheck = 0x10
}

[Flags]
public enum Expcmdflags : uint {
    Default = 0,
    Hassubcommands = 0x1,
    Hassplitbutton = 0x2,
    Hidelabel = 0x4,
    Isseparator = 0x8,
    Hasluashield = 0x10,
    Separatorbefore = 0x20,
    Separatorafter = 0x40,
    Isdropdown = 0x80,
    Toggleable = 0x100,
    Automenuicons = 0x200
}

[GeneratedComInterface]
[Guid("a08ce4d0-fa25-44ab-b57c-c7b1c323e0b9")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public unsafe partial interface IExplorerCommand {
    [return: MarshalAs(UnmanagedType.LPWStr)]
    string? GetTitle(IShellItemArray? itemArray);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string? GetIcon(IShellItemArray? itemArray);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string? GetToolTip(IShellItemArray? itemArray);

    Guid GetCanonicalName();

    Expcmdstate GetState(IShellItemArray? itemArray, [MarshalAs(UnmanagedType.Bool)] bool okToBeSlow);

    void Invoke(IShellItemArray? itemArray, void* bc);

    Expcmdflags GetFlags();

    void* EnumSubCommands();
}

[GeneratedComClass]
[Guid("059F8A2D-271B-415E-9267-18B9E4B164DC")]
public sealed unsafe partial class TheCommand : IExplorerCommand {
    public string? GetTitle(IShellItemArray? itemArray) => "TestCommand";

    public string? GetIcon(IShellItemArray? itemArray) => throw new NotImplementedException();

    public string? GetToolTip(IShellItemArray? itemArray) => throw new NotImplementedException();

    public Guid GetCanonicalName() => throw new NotImplementedException();

    public Expcmdstate GetState(IShellItemArray? itemArray, bool okToBeSlow) => Expcmdstate.Enabled;

    public void Invoke(IShellItemArray? itemArray, void* bc) {
        var mh = GetModuleHandleW("ComTest.dll");

        if (mh is null) {
            MessageBoxW(null, $"GetModuleHandleW Failed: {Marshal.GetLastPInvokeErrorMessage()}", null, 16);

            return;
        }

        const uint size = byte.MaxValue;
        var sb = stackalloc char[(int)size];

        if (GetModuleFileNameW(mh, sb, size) == 0) {
            MessageBoxW(null, $"GetModuleFileNameW Failed: {Marshal.GetLastPInvokeErrorMessage()}", null, 16);

            return;
        }

        var displayName = itemArray?.GetItemAt(0)?.GetDisplayName(Sigdn.Desktopabsoluteediting);

        if (displayName is null) {
            MessageBoxW(null, "File name", null, 16);

            return;
        }

        var p = Process.Start(Path.Combine(Path.GetDirectoryName(new(sb))!, "App1.exe"), displayName);

        if (p is null) {
            MessageBoxW(null, "App1 launch failed.", null, 16);
        }
    }

    public Expcmdflags GetFlags() => Expcmdflags.Default;

    public void* EnumSubCommands() => throw new NotImplementedException();

    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern int MessageBoxW(
        void* hwnd,
        [MarshalAs(UnmanagedType.LPWStr)] string? text,
        [MarshalAs(UnmanagedType.LPWStr)] string? caption,
        uint type);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern uint GetModuleFileNameW(void* module, char* filename, uint size);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern void* GetModuleHandleW([MarshalAs(UnmanagedType.LPWStr)] string moduleName);
}

[GeneratedComClass]
[ClassFactory<TheCommand>]
internal sealed partial class TheCommandFactory : IClassFactory;

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ComTest;

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
    string? GetTitle(void* itemArray);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string? GetIcon(void* itemArray);

    [return: MarshalAs(UnmanagedType.LPWStr)]
    string? GetToolTip(void* itemArray);

    Guid GetCanonicalName();

    Expcmdstate GetState(void* itemArray, [MarshalAs(UnmanagedType.Bool)] bool okToBeSlow);

    void Invoke(void* itemArray, void* bc);

    Expcmdflags GetFlags();

    void** EnumSubCommands();
}

[GeneratedComClass]
[Guid("059F8A2D-271B-415E-9267-18B9E4B164DC")]
public sealed unsafe partial class TheCommand : IExplorerCommand {
    public string? GetTitle(void* itemArray) => "TestCommand";

    public string? GetIcon(void* itemArray) => throw new NotImplementedException();

    public string? GetToolTip(void* itemArray) => throw new NotImplementedException();

    public Guid GetCanonicalName() => throw new NotImplementedException();

    public Expcmdstate GetState(void* itemArray, bool okToBeSlow) => Expcmdstate.Enabled;

    public void Invoke(void* itemArray, void* bc) => MessageBoxW(IntPtr.Zero, "Hello, World!", string.Empty, 64);

    public Expcmdflags GetFlags() => Expcmdflags.Default;

    public void** EnumSubCommands() => throw new NotImplementedException();

    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    private static extern int MessageBoxW(IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] string text, [MarshalAs(UnmanagedType.LPWStr)] string caption, uint type);
}

[GeneratedComInterface]
[Guid("00000001-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public unsafe partial interface IClassFactory {
    [PreserveSig]
    int CreateInstance(void* pUnkOuter, Guid* riid, void** ppvObject);

    [PreserveSig]
    int LockServer([MarshalAs(UnmanagedType.Bool)] bool fLock);
}

[GeneratedComClass]
[Guid("3B545406-898A-4FC7-8D87-A9E84D72CFBC")]
public unsafe partial class ClassFactory : IClassFactory {
    private static readonly Guid IecIid = Guid.Parse("a08ce4d0-fa25-44ab-b57c-c7b1c323e0b9");
    private static volatile int ServerLocks;

    public static bool IsLocked => ServerLocks > 0;

    public int CreateInstance(void* pUnkOuter, Guid* riid, void** ppvObject) {
        if (*riid != IecIid) {
            return unchecked((int)0x80040111); // CLASS_E_CLASSNOTAVAILABLE
        }

        StrategyBasedComWrappers sbcw = new();
        TheCommand tc = new();

        var ptr = sbcw.GetOrCreateComInterfaceForObject(tc, CreateComInterfaceFlags.None);

        try {
            var hr = Marshal.QueryInterface(ptr, in *riid, out var pInterface);

            if (hr != 0) {
                return hr;
            }

            if (ptr == IntPtr.Zero) {
                return unchecked((int)0x8000FFFF); // E_UNEXPECTED
            }

            *ppvObject = pInterface.ToPointer();

            return 0; // S_OK
        } finally {
            Marshal.Release(ptr);
        }
    }

    public int LockServer(bool fLock) {
        if (fLock) {
            Interlocked.Increment(ref ServerLocks);
        } else {
            Interlocked.Decrement(ref ServerLocks);
        }

        return 0;
    }
}

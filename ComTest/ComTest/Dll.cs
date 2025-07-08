using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace ComTest;

public static unsafe class Dll {
    private static readonly Guid IID_IClassFactory = Guid.Parse("00000001-0000-0000-C000-000000000046");

    [UnmanagedCallersOnly(EntryPoint = nameof(DllGetClassObject))]
    public static int DllGetClassObject(Guid* clsid, Guid* iid, void** ppv) {
        *ppv = null;

        if (*iid != IID_IClassFactory) {
            return unchecked((int)0x80040111); // CLASS_E_CLASSNOTAVAILABLE
        }

        StrategyBasedComWrappers sbcw = new();
        ClassFactory cf = new();

        var ptr = sbcw.GetOrCreateComInterfaceForObject(cf, CreateComInterfaceFlags.None);

        try {
            var hr = Marshal.QueryInterface(ptr, in *iid, out var pInterface);

            if (hr != 0) {
                return hr;
            }

            if (ptr == IntPtr.Zero) {
                return unchecked((int)0x8000FFFF); // E_UNEXPECTED
            }

            *ppv = pInterface.ToPointer();

            return 0; // S_OK
        } finally {
            Marshal.Release(ptr);
        }
    }

    [UnmanagedCallersOnly(EntryPoint = nameof(DllCanUnloadNow))]
    public static int DllCanUnloadNow() => ClassFactory.IsLocked.GetHashCode();
}

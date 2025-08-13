using Microsoft.Win32;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WA.Cbs;

internal static class CbsManager {
    private static readonly Guid IID_ICbsSession = new("75207391-23f2-4396-85f0-8fdb879ed0ed");
    private static bool cbsInitialized;
    private static IClassFactory? cbsSessionClassFactory;
    private static ICbsSession? currentSession;

    public static void LoadCbs() {
        string ssPath;

        var regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Component Based Servicing\Version")
            ?? throw new InvalidOperationException("Servicing stack not found.");

        using (regkey) {
            Version? latestVersion = null;

            foreach (var valueName in regkey.GetValueNames()) {
                Version v = new(valueName);

                if (latestVersion is null || v > latestVersion) {
                    latestVersion = v;
                }
            }

            ssPath = regkey.GetValue(latestVersion!.ToString()).ToString();
        }

        if (!File.Exists(Path.Combine(ssPath, "cbscore.dll"))) {
            throw new InvalidOperationException("Servicing stack not found.");
        }

        if (!SetDllDirectoryW(ssPath)) {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public static unsafe void InitializeCbs() {
        if (cbsInitialized) {
            throw new InvalidOperationException("CBS already initialized.");
        }

        void* pMalloc = null;
        void* pCf = null;

        try {
            var cgm = CoGetMalloc(1, &pMalloc);

            if (cgm != 0 || pMalloc is null) {
                throw new Win32Exception(cgm);
            }

            var cci = CbsCoreInitialize(pMalloc, &NilFunc1, &NilFunc2, &NilFunc2, &NilFunc2, &NilFunc2, &NilFunc2, &pCf);

            if (cci != 0 || pCf is null) {
                throw new Win32Exception(cci);
            }

            cbsSessionClassFactory = (IClassFactory)Marshal.GetTypedObjectForIUnknown((IntPtr)pCf, typeof(IClassFactory));
            cbsInitialized = true;
        } finally {
            if (pMalloc is not null) {
                Marshal.Release((IntPtr)pMalloc);
            }

            if (pCf is not null) {
                Marshal.Release((IntPtr)pCf);
            }
        }
    }

    public static unsafe ICbsSession GetCbsSession() {
        if (!cbsInitialized || cbsSessionClassFactory is null) {
            throw new InvalidOperationException("CBS not initialized.");
        }

        if (currentSession is not null) {
            return currentSession;
        }

        void* pCs = null;

        fixed (Guid* ics = &IID_ICbsSession) {
            var ci = cbsSessionClassFactory.CreateInstance(null, ics, &pCs);

            if (ci != 0) {
                throw new Win32Exception(ci);
            }
        }

        currentSession = (ICbsSession)Marshal.GetTypedObjectForIUnknown((IntPtr)pCs, typeof(ICbsSession));

        return currentSession;
    }

    public static void CloseCbsSession() {
        if (currentSession is null) {
            throw new InvalidOperationException("No session.");
        }

        Marshal.ReleaseComObject(currentSession);
    }

    public static void UninitializeCbs() {
        if (!cbsInitialized) {
            throw new InvalidOperationException("CBS not initialized.");
        }

        var ccf = CbsCoreFinalize();

        if (ccf != 0) {
            throw new Win32Exception(ccf);
        }

        cbsInitialized = false;
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetDllDirectoryW([MarshalAs(UnmanagedType.LPWStr)] string? lpPathName);

    [DllImport("cbscore.dll", ExactSpelling = true)]
    private static extern unsafe int CbsCoreInitialize(
        void* /* IMalloc* */ malloc,
        delegate* unmanaged[Stdcall]<int, int> lockProcess,
        delegate* unmanaged[Stdcall]<void> unlockProcess,
        delegate* unmanaged[Stdcall]<void> instanceCreated,
        delegate* unmanaged[Stdcall]<void> instanceDestroyed,
        delegate* unmanaged[Stdcall]<void> requireShutdownNow,
        delegate* unmanaged[Stdcall]<void> requireShutdownProcessing,
        void** /* IClassFactory** */ classFactory);

    [DllImport("cbscore.dll", ExactSpelling = true)]
    private static extern int CbsCoreFinalize();

    [DllImport("ole32.dll", ExactSpelling = true)]
    private static extern unsafe int CoGetMalloc(uint memContext, void** /* IMalloc** */ malloc);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static int NilFunc1(int i) => 0;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvStdcall)])]
    private static void NilFunc2() { }
}

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SimpleMultiReboot;

internal static class Program {
    public static void SetFirmwareVariable() {
        EnablePrivilege("SeSystemEnvironmentPrivilege");

        var ptr = Marshal.AllocHGlobal(Marshal.SizeOf<long>());

        Marshal.WriteInt64(ptr, 1);

        try {
            if (!NativeMethods.SetFirmwareEnvironmentVariableExW("OsIndications", "{8BE4DF61-93CA-11D2-AA0D-00E098032B8C}", ptr, 8, 7)) {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        } finally {
            Marshal.FreeHGlobal(ptr);
        }
    }

    public static void RebootComputer() {
        EnablePrivilege("SeShutdownPrivilege");

        if (!NativeMethods.InitiateSystemShutdownW(null, null, 0, true, true)) {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }
    }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main() {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }

    private static void EnablePrivilege(string privilegeName) {
        if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().SafeHandle, 40U, out var token)) {
            Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
        }

        try {
            if (!NativeMethods.LookupPrivilegeValueW(null, privilegeName, out var luid)) {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }

            LUID_AND_ATTRIBUTES laa = new() {
                Attributes = 2,
                Luid = luid
            };

            TOKEN_PRIVILEGES tp = new() {
                PrivilegeCount = 1,
                Privileges = laa
            };

            if (!NativeMethods.AdjustTokenPrivileges(token, false, in tp, (uint)Marshal.SizeOf<TOKEN_PRIVILEGES>(), out _, out _)) {
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
        } finally {
            token.Dispose();
        }
    }
}

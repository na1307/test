using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace SimpleMultiReboot;

internal static partial class NativeMethods {
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetFirmwareType(ref int firmwareType);

    [LibraryImport("kernel32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetFirmwareEnvironmentVariableExW(string name, string guid, nint value, uint size, uint attributes);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool OpenProcessToken(SafeProcessHandle handle, uint desiredAccess, out SafeAccessTokenHandle tokenHandle);

    [LibraryImport("advapi32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool LookupPrivilegeValueW(string? systemName, string name, out LUID luid);

    [LibraryImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AdjustTokenPrivileges(
        SafeAccessTokenHandle handle,
        [MarshalAs(UnmanagedType.Bool)] bool disableAllPrivileges,
        in TOKEN_PRIVILEGES newState,
        uint bufferLength,
        out TOKEN_PRIVILEGES previousState,
        out uint returnLength);

    [LibraryImport("advapi32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InitiateSystemShutdownW(
        string? machineName,
        string? message,
        uint timeout,
        [MarshalAs(UnmanagedType.Bool)] bool forceAppClosed,
        [MarshalAs(UnmanagedType.Bool)] bool rebootAfterShutdown);
}

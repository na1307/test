namespace Bluehill.Bcd;

internal static class ExceptionExtensions {
    public static int GetHResult(this Exception exception) {
#if NET45_OR_GREATER || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER
        return exception.HResult;
#else
        return System.Runtime.InteropServices.Marshal.GetHRForException(exception);
#endif
    }
}

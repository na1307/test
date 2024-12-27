using System.Runtime.InteropServices;

namespace Etude;

internal static partial class NativeMethods {
    [LibraryImport("c", EntryPoint = "getuid")]
    public static partial uint GetUID();
}

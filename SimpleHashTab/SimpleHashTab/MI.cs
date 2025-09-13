using System.Runtime.CompilerServices;
using WinFormsComInterop;

namespace SimpleHashTab;

internal static class MI {
#pragma warning disable CA2255 // 'ModuleInitializer' 특성은 라이브러리에서 사용하면 안 됩니다.
    [ModuleInitializer]
#pragma warning restore CA2255 // 'ModuleInitializer' 특성은 라이브러리에서 사용하면 안 됩니다.
    internal static void ModuleInitializer() => ComWrappers.RegisterForMarshalling(WinFormsComWrappers.Instance);
}

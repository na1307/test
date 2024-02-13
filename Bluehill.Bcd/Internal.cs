using System.Management;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace Bluehill.Bcd;

internal static class Internal {
    public const string ScopeString = "root\\WMI";

    private static bool isAdmin => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void AdminCheck() {
        if (!isAdmin) {
            throw new RequiresAdministratorException();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReturnValueCheck(ManagementBaseObject outParam) {
        if (!(bool)outParam["ReturnValue"]) {
            throw new BcdException();
        }
    }
}

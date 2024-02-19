using System.Management;
using System.Security.Principal;

namespace Bluehill.Bcd;

internal static class Internal {
    public const string ScopeString = "root\\WMI";

    private static bool isAdmin => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    public static void AdminCheck() {
        if (!isAdmin) {
            throw new RequiresAdministratorException();
        }
    }

    public static void ReturnValueCheck(ManagementBaseObject outParam) {
        if (!(bool)outParam["ReturnValue"]) {
            throw new BcdException();
        }
    }
}

using System.ComponentModel;

namespace Bluehill.Vhd;

/// <summary>
/// Provides VHD-relates Methods.
/// </summary>
public static partial class VhdFunctions {
    private static VirtualStorageType getVst(string path) {
        var extension = Path.GetExtension(path).TrimStart('.');
        VirtualStorageTypeDevice deviceId;

        if (extension.Equals("vhd", StringComparison.OrdinalIgnoreCase)) {
            deviceId = VirtualStorageTypeDevice.Vhd;
        } else if (extension.Equals("vhdx", StringComparison.OrdinalIgnoreCase)) {
            deviceId = VirtualStorageTypeDevice.Vhdx;
        } else {
            throw new ArgumentException("File extension is invalid.");
        }

        return new() {
            DeviceId = deviceId,
            VendorId = VirtualStorageType.Microsoft
        };
    }

    private static string getMessageFromErrorCode(uint errorCode) => new Win32Exception((int)errorCode).Message;
}

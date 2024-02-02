using System.Management;
using static Bluehill.Bcd.Internal;

namespace Bluehill.Bcd;

public sealed record class BcdStore {
    private const string PathStartString = "BcdStore.FilePath='";
    private const string PathEndString = "'";
    private static readonly ManagementObject staticInstance = new(ScopeString, PathStartString + PathEndString, null);

    internal BcdStore(string fp) => FilePath = fp;

    public static BcdStore SystemStore { get; } = new(string.Empty);
    public string FilePath { get; }
    public bool IsSystemStore => string.IsNullOrEmpty(FilePath);

    public static BcdStore CreateStore(string filePath) {
        if (string.IsNullOrEmpty(filePath)) {
            throw new ArgumentException($"'{nameof(filePath)}'은(는) Null이거나 비워 둘 수 없습니다.", nameof(filePath));
        }

        if (File.Exists(filePath)) {
            throw new BcdException("File already exists.");
        }

        AdminCheck();

        try {
            ManagementBaseObject inParam = staticInstance.GetMethodParameters(nameof(CreateStore));
            inParam["File"] = filePath;

            ManagementBaseObject outParam = staticInstance.InvokeMethod(nameof(CreateStore), inParam, null);
            ReturnValueCheck(outParam);

            ManagementBaseObject createdStore = (ManagementBaseObject)outParam["Store"];

            return new((string)createdStore[nameof(FilePath)]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public static BcdStore OpenStore(string filePath) {
        if (string.IsNullOrEmpty(filePath)) {
            throw new ArgumentException($"'{nameof(filePath)}'은(는) Null이거나 비워 둘 수 없습니다.", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException("The specified file cannot be found.", nameof(filePath));
        }

        AdminCheck();

        try {
            ManagementBaseObject inParam = staticInstance.GetMethodParameters(nameof(OpenStore));
            inParam["File"] = filePath;

            ManagementBaseObject outParam = staticInstance.InvokeMethod(nameof(OpenStore), inParam, null);
            ReturnValueCheck(outParam);

            ManagementBaseObject store = (ManagementBaseObject)outParam["Store"];

            return new((string)store[nameof(FilePath)]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public BcdObject OpenObject(Guid id) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(OpenObject));
            inParam["Id"] = id.ToString("B");

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(OpenObject), inParam, null);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public BcdObject CreateObject(Guid id, BcdObjectType type) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(CreateObject));
            inParam["Id"] = id.ToString("B");
            inParam["Type"] = type;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(CreateObject), inParam, null);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public BcdObject CopyObject(BcdObject source, bool deleteExistingObject = false) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(CopyObject));
            inParam["SourceStoreFile"] = source.Store.FilePath;
            inParam["SourceId"] = source.Id.ToString("B");
            inParam["Flags"] = deleteExistingObject ? 0x2 : 0x1;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(CopyObject), inParam, null);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }
}

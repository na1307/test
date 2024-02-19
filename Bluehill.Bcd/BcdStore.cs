using System.Management;
using System.Runtime.CompilerServices;
using static Bluehill.Bcd.Internal;

namespace Bluehill.Bcd;

/// <summary>
/// Represents a BCD store that contains a collection of BCD objects.
/// </summary>
public sealed record class BcdStore {
    private const string pathStartString = "BcdStore.FilePath='";
    private const string pathEndString = "'";
    private static readonly ManagementObject staticInstance = new(ScopeString, pathStartString + pathEndString, null);
    private static readonly BcdStore systemStore = new(string.Empty);
    private readonly ManagementObject classInstance;

    internal BcdStore(string fp) {
        FilePath = fp;
        classInstance = new(ScopeString, $"{pathStartString}{fp}{pathEndString}", null);
    }

    /// <summary>
    /// The system store.
    /// </summary>
    public static BcdStore SystemStore {
        get {
            AdminCheck();
            return systemStore;
        }
    }

    /// <summary>
    /// A file path that uniquely identifies the store. The system store is denoted by an empty string.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Whether this store is a system store.
    /// </summary>
    public bool IsSystemStore => string.IsNullOrEmpty(FilePath);

    /// <summary>
    /// Creates a new store.
    /// </summary>
    /// <param name="filePath">The full path to the store to be created.</param>
    /// <returns>A BcdStore object.</returns>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is <see langword="null"/> or empty.</exception>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public static BcdStore CreateStore(string filePath) {
        if (string.IsNullOrEmpty(filePath)) {
            throw new ArgumentException($"'{nameof(filePath)}'은(는) Null이거나 비워 둘 수 없습니다.", nameof(filePath));
        }

        if (File.Exists(filePath)) {
            throw new BcdException("File already exists.");
        }

        AdminCheck();

        try {
            var inParam = staticInstance.GetMethodParameters(nameof(CreateStore));
            inParam["File"] = filePath;

            var outParam = staticInstance.InvokeMethod(nameof(CreateStore), inParam, null);
            ReturnValueCheck(outParam);

            var createdStore = (ManagementBaseObject)outParam["Store"];

            return new((string)createdStore[nameof(FilePath)]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Opens a store.
    /// </summary>
    /// <param name="filePath">The full path to the store to be opened.</param>
    /// <returns>A BcdStore object.</returns>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> is <see langword="null"/> or empty.</exception>
    /// <exception cref="FileNotFoundException">File is already exists.</exception>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public static BcdStore OpenStore(string filePath) {
        if (string.IsNullOrEmpty(filePath)) {
            throw new ArgumentException($"'{nameof(filePath)}'은(는) Null이거나 비워 둘 수 없습니다.", nameof(filePath));
        }

        if (!File.Exists(filePath)) {
            throw new FileNotFoundException("The specified file cannot be found.", nameof(filePath));
        }

        AdminCheck();

        try {
            var inParam = staticInstance.GetMethodParameters(nameof(OpenStore));
            inParam["File"] = filePath;

            var outParam = staticInstance.InvokeMethod(nameof(OpenStore), inParam, null);
            ReturnValueCheck(outParam);

            var store = (ManagementBaseObject)outParam["Store"];

            return new((string)store[nameof(FilePath)]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Opens the specified object.
    /// </summary>
    /// <param name="id">The object identifier.</param>
    /// <returns>The object.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public BcdObject OpenObject(Guid id) {
        AdminCheck();

        try {
            var inParam = getInParam();
            inParam["Id"] = id.ToString("B");

            var outParam = getOutParam(inParam);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Creates the specified object.
    /// </summary>
    /// <param name="id">The object identifier.</param>
    /// <param name="type">The object type.</param>
    /// <returns>The object.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public BcdObject CreateObject(Guid id, BcdObjectType type) {
        AdminCheck();

        try {
            var inParam = getInParam();
            inParam["Id"] = id.ToString("B");
            inParam["Type"] = type;

            var outParam = getOutParam(inParam);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Copies the specified object from another store.
    /// </summary>
    /// <param name="source">The object of the object to copy.</param>
    /// <param name="flags">The <see cref="CopyObjectOptions"/> flags.</param>
    /// <returns>A Bcdobject instance that represents the copied object.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public BcdObject CopyObject(BcdObject source, CopyObjectOptions flags) {
        AdminCheck();

        try {
            var inParam = getInParam();
            inParam["SourceStoreFile"] = source.Store.FilePath;
            inParam["SourceId"] = source.Id.ToString("B");
            inParam["Flags"] = flags;

            var outParam = getOutParam(inParam);
            ReturnValueCheck(outParam);

            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, Guid.Parse((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    private ManagementBaseObject getInParam([CallerMemberName] string? methodName = default) => classInstance.GetMethodParameters(methodName);
    private ManagementBaseObject getOutParam(ManagementBaseObject inParam, [CallerMemberName] string? methodName = default) => classInstance.InvokeMethod(methodName, inParam, null);
}

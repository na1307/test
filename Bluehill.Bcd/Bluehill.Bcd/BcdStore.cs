﻿using System.Management;
using System.Runtime.CompilerServices;
using static Bluehill.Bcd.Internal;

namespace Bluehill.Bcd;

/// <summary>
/// Represents a BCD store that contains a collection of BCD objects.
/// </summary>
public sealed record class BcdStore : IDisposable {
    private const string pathStartString = "BcdStore.FilePath='";
    private const string pathEndString = "'";
    private static readonly ManagementObject staticInstance = new(ScopeString, pathStartString + pathEndString, null);
    private static readonly BcdStore systemStore = new(string.Empty);
    private readonly ManagementObject classInstance;
    private bool disposedValue;

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
        check();

        try {
            var inParam = getInParam();
            inParam["Id"] = id.ToString("B");

            var outParam = getOutParam(inParam);
            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, new Guid((string)bo["id"]), (BcdObjectType)(uint)bo["Type"]);
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
        check();

        try {
            var inParam = getInParam();
            inParam["Id"] = id.ToString("B");
            inParam["Type"] = type;

            var outParam = getOutParam(inParam);
            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, new Guid((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
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
        check();

        try {
            var inParam = getInParam();
            inParam["SourceStoreFile"] = source.Store.FilePath;
            inParam["SourceId"] = source.Id.ToString("B");
            inParam["Flags"] = flags;

            var outParam = getOutParam(inParam);
            var bo = (ManagementBaseObject)outParam["Object"];

            return new(this, new Guid((string)bo["Id"]), (BcdObjectType)(uint)bo["Type"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <inheritdoc/>
    public void Dispose() {
        if (IsSystemStore) {
            throw new InvalidOperationException("The system store cannot be disposed.");
        }

        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public bool Equals(BcdStore? other) => ReferenceEquals(this, other) || (other is not null && FilePath == other.FilePath);

    /// <summary>
    /// Returns the hash code of this object.
    /// </summary>
    /// <returns>The hash code of this object.</returns>
    public override int GetHashCode() => FilePath.GetHashCode();

    private void Dispose(bool disposing) {
        if (!disposedValue) {
            if (disposing) {
                classInstance.Dispose();
            }

            disposedValue = true;
        }
    }

    private void check() {
        if (disposedValue) {
            throw new ObjectDisposedException(nameof(BcdStore));
        }

        AdminCheck();
    }

    private ManagementBaseObject getInParam([CallerMemberName] string? methodName = default) => classInstance.GetMethodParameters(methodName);

    private ManagementBaseObject getOutParam(ManagementBaseObject inParam, [CallerMemberName] string? methodName = default) {
        var outParam = classInstance.InvokeMethod(methodName, inParam, null);
        ReturnValueCheck(outParam);
        return outParam;
    }
}

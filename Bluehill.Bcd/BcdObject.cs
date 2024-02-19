using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Bluehill.Bcd.Internal;

namespace Bluehill.Bcd;

/// <summary>
/// Represents a BCD object that contains a collection of BCD elements. Each BCD object is identified by a GUID.
/// </summary>
public sealed record class BcdObject {
    private const string pathStartString = "BcdObject.Id='";
    private const string pathMiddleString = "',StoreFilePath='";
    private const string pathEndString = "'";

    internal BcdObject(BcdStore store, Guid id, BcdObjectType type) {
        Store = store;
        Id = id;
        Type = type;
    }

    /// <summary>
    /// The store.
    /// </summary>
    public BcdStore Store { get; }

    /// <summary>
    /// The GUID of this object, unique to this store, in string form.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The object type.
    /// </summary>
    public BcdObjectType Type { get; }

    /// <summary>
    /// Enumerates the elements in the object.
    /// </summary>
    /// <returns>An array of elements.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public object[] EnumerateElements() {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(EnumerateElements));
            var outParam = classInstance.InvokeMethod(nameof(EnumerateElements), inParam, null);
            ReturnValueCheck(outParam);

            return ((ManagementBaseObject[])outParam["Elements"]).Select(getElement).ToArray();
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Enumerates the types of elements in the object.
    /// </summary>
    /// <returns>An array of element types.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public BcdElementType[] EnumerateElementTypes() {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(EnumerateElementTypes));
            var outParam = classInstance.InvokeMethod(nameof(EnumerateElementTypes), inParam, null);
            ReturnValueCheck(outParam);

            return ((uint[])outParam["Types"]).Select(i => (BcdElementType)i).ToArray();
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Gets the specified element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <returns>The element.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public object GetElement(BcdElementType type) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(GetElement));
            inParam["Type"] = type;

            var outParam = classInstance.InvokeMethod(nameof(GetElement), inParam, null);
            ReturnValueCheck(outParam);

            return getElement((ManagementBaseObject)outParam["Element"]);
        } catch (ManagementException err) {
            throw new BcdException(err);
        } catch (COMException cex) when (cex.HResult == -805305819) {
            throw new BcdException("Element not found.", cex);
        }
    }

    /// <summary>
    /// Gets the specified element. This method can be used to enumerate a qualified partition.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="flags">The flags.</param>
    /// <returns>The element.</returns>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public object GetElementWithFlags(BcdElementType type, GetElementWithFlagsOptions flags) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(GetElementWithFlags));
            inParam["Type"] = type;
            inParam["Flags"] = flags;

            var outParam = classInstance.InvokeMethod(nameof(GetElementWithFlags), inParam, null);
            ReturnValueCheck(outParam);

            foreach (var property in ((ManagementBaseObject)outParam["Element"]).Properties) {
                switch (property.Name) {
                    case "String" or "Integer" or "Boolean" or "Integers":
                        return property.Value;

                    case "Device":
                        return getDeviceData((ManagementBaseObject)property.Value);

                    case "Id":
                        return Store.OpenObject(Guid.Parse((string)property.Value));

                    case "Ids":
                        return ((string[])property.Value).Select(s => Store.OpenObject(Guid.Parse(s))).ToArray();

                    case "StoreFilePath" or "ObjectId" or "Type":
                        break;

                    default:
                        throw new BcdException("Unknown " + property.Name);
                }
            }

            throw new BcdException("Unknown1");
        } catch (ManagementException err) {
            throw new BcdException(err);
        } catch (COMException cex) when (cex.HResult == -805305819) {
            throw new BcdException("Element not found.", cex);
        }
    }

    /// <summary>
    /// Sets the specified integer element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="integer">The element value.</param>
    public void SetIntegerElement(BcdElementType type, long integer) => setElement(type, (ulong)integer, "Integer");

    /// <summary>
    /// Sets the specified Boolean element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="boolean">The element value.</param>
    public void SetBooleanElement(BcdElementType type, bool boolean) => setElement(type, boolean, "Boolean");

    /// <summary>
    /// Sets the specified string element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="string">The element value.</param>
    public void SetStringElement(BcdElementType type, string @string) => setElement(type, @string, "String");

    /// <summary>
    /// Sets the specified partition device element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="deviceType">The device type.</param>
    /// <param name="additionalOptions">Either another object in the store, or <see langword="null"/>.</param>
    /// <param name="path">The partition path.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetPartitionDeviceElement(BcdElementType type, DeviceType deviceType, BcdObject? additionalOptions, string path) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetPartitionDeviceElement));
            inParam["Type"] = type;
            inParam["DeviceType"] = deviceType;
            inParam["AdditionalOptions"] = additionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["Path"] = path;

            var outParam = classInstance.InvokeMethod(nameof(SetPartitionDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Sets the specified partition device element. This method is identical to <see cref="SetPartitionDeviceElement"/> except it takes additional flags.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="deviceType">The device type.</param>
    /// <param name="additionalOptions">Either another object in the store, or <see langword="null"/>.</param>
    /// <param name="path">The partition path.</param>
    /// <param name="flags">The flags.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetPartitionDeviceElementWithFlags(BcdElementType type, DeviceType deviceType, BcdObject? additionalOptions, string path, SetPartitionDeviceElementWithFlagsOptions flags) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetPartitionDeviceElementWithFlags));
            inParam["Type"] = type;
            inParam["DeviceType"] = deviceType;
            inParam["AdditionalOptions"] = additionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["Path"] = path;
            inParam["Flags"] = flags;

            var outParam = classInstance.InvokeMethod(nameof(SetPartitionDeviceElementWithFlags), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Sets the specified file device element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="deviceType">The device type.</param>
    /// <param name="additionalOptions">Either another object in the store, or <see langword="null"/>.</param>
    /// <param name="path">The file path.</param>
    /// <param name="parentDeviceType">The device type.</param>
    /// <param name="parentAdditionalOptions">Either another object in the store, or <see langword="null"/>.</param>
    /// <param name="parentPath">The path of the parent. This parameter can be an empty string if the parent device is of a type that does not have a path.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetFileDeviceElement(BcdElementType type, DeviceType deviceType, BcdObject? additionalOptions, string path, DeviceType parentDeviceType, BcdObject? parentAdditionalOptions, string parentPath) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetFileDeviceElement));
            inParam["Type"] = type;
            inParam["DeviceType"] = deviceType;
            inParam["AdditionalOptions"] = additionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["Path"] = path;
            inParam["ParentDeviceType"] = parentDeviceType;
            inParam["ParentAdditionalOptions"] = parentAdditionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["ParentPath"] = parentPath;

            var outParam = classInstance.InvokeMethod(nameof(SetFileDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Creates a virtual hard disk (VHD) boot device element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="path">The path to the VHD file.</param>
    /// <param name="parentDeviceType">The device type.</param>
    /// <param name="parentAdditionalOptions">Either another object in the store, or <see langword="null"/>.</param>
    /// <param name="parentPath">The path to the physical partition that contains the VHD file. If the <paramref name="parentDeviceType"/> parameter is <see cref="DeviceType.LocateDevice"/>, this parameter is not used.</param>
    /// <param name="customLocate">A BCD element that overrides the default locate heuristics for a VHD device.<br/><br/>If this parameter is zero, the application path (for example, \Windows\System32\winload.exe) is used to locate a boot device and the system root element (\Windows) is used to locate an operating system device.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetVhdDeviceElement(BcdElementType type, string path, DeviceType parentDeviceType, BcdObject? parentAdditionalOptions, string parentPath, long customLocate) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetVhdDeviceElement));
            inParam["Type"] = type;
            inParam["Path"] = path;
            inParam["ParentDeviceType"] = parentDeviceType;
            inParam["ParentAdditionalOptions"] = parentAdditionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["ParentPath"] = parentPath;
            inParam["CustomLocate"] = customLocate;

            var outParam = classInstance.InvokeMethod(nameof(SetVhdDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Sets a qualified boot partition device.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="partitionStyle">The partition style.</param>
    /// <param name="diskSignature">If the PartitionStyle parameter is GPT, the DiskSignature parameter is the disk signature as a GUID in string format (for example, "{7c69a706-eda5-11dd-bc09-001e4ce28b8f}"). If the PartitionStyle parameter is MBR, the DiskSignature parameter is the decimal MBR disk signature in string format (for example, "402653184" for 0x18000000).</param>
    /// <param name="partitionIdentifer">If the PartitionStyle parameter is GPT, the PartitionIdentifier parameter is the partition signature as a GUID in string format (for example, "{6efb52bf-1766-41db-a6b3-0ee5eff72bd7}" ). If the PartitionStyle parameter is MBR, the PartitionIdentifier parameter is the decimal MBR partition offset in string format (for example, "82837504" for 0x4F00000).</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetQualifiedPartitionDeviceElement(BcdElementType type, PartitionStyle partitionStyle, string diskSignature, string partitionIdentifer) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetQualifiedPartitionDeviceElement));
            inParam["Type"] = type;
            inParam["PartitionStyle"] = partitionStyle;
            inParam["DiskSignature"] = diskSignature;
            inParam["PartitionIdentifier"] = partitionIdentifer;

            var outParam = classInstance.InvokeMethod(nameof(SetQualifiedPartitionDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Sets the specified object element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="object">The object.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetObjectElement(BcdElementType type, BcdObject @object) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetObjectElement));
            inParam["Type"] = type;
            inParam["Id"] = @object.Id.ToString("B");

            var outParam = classInstance.InvokeMethod(nameof(SetObjectElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Sets the specified object list element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <param name="objects">An array of objects.</param>
    /// <exception cref="ArgumentException"><paramref name="objects"/> is <see langword="null"/> or empty</exception>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void SetObjectListElement(BcdElementType type, params BcdObject[] objects) {
        if (objects is null || objects.Length == 0) {
            throw new ArgumentException($"'{nameof(objects)}' cannot be Null or empty.", nameof(objects));
        }

        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(SetObjectListElement));
            inParam["Type"] = type;
            inParam["Ids"] = objects.Select(o => o.Id.ToString("B")).ToArray();

            var outParam = classInstance.InvokeMethod(nameof(SetObjectListElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    /// <summary>
    /// Deletes the specified element.
    /// </summary>
    /// <param name="type">The element type.</param>
    /// <exception cref="BcdException">Error occurred during BCD wMI operation</exception>
    public void DeleteElement(BcdElementType type) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(nameof(DeleteElement));
            inParam["Type"] = type;

            var outParam = classInstance.InvokeMethod(nameof(DeleteElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    private object getElement(ManagementBaseObject element) {
        foreach (var property in element.Properties) {
            switch (property.Name) {
                case "String" or "Integer" or "Boolean" or "Integers":
                    return property.Value;

                case "Device":
                    return getDeviceData((ManagementBaseObject)property.Value);

                case "Id":
                    return Store.OpenObject(Guid.Parse((string)property.Value));

                case "Ids":
                    return ((string[])property.Value).Select(s => Store.OpenObject(Guid.Parse(s))).ToArray();

                case "StoreFilePath" or "ObjectId" or "Type":
                    break;

                default:
                    throw new BcdException("Unknown " + property.Name);
            }
        }

        throw new BcdException("Unknown1");
    }

    private BcdDeviceData getDeviceData(ManagementBaseObject deviceData) {
        var type = (DeviceType)(uint)deviceData["DeviceType"];
        var addOptionsString = (string)deviceData["AdditionalOptions"];
        var addOptions = string.IsNullOrEmpty(addOptionsString) ? null : Store.OpenObject(Guid.Parse(addOptionsString));

        foreach (var property in deviceData.Properties) {
            switch (property.Origin) {
                case nameof(BcdDevicePartitionData):
                    return new BcdDevicePartitionData(type, addOptions, (string)deviceData["Path"]);

                case nameof(BcdDeviceFileData):
                    return new BcdDeviceFileData(type, addOptions, getDeviceData((ManagementBaseObject)deviceData["Parent"]), (string)deviceData["Path"]);

                case nameof(BcdDeviceLocateStringData):
                    return new BcdDeviceLocateStringData(type, addOptions, (LocateDeviceType)(uint)deviceData["Type"], (string)deviceData["Path"]);

                case nameof(BcdDeviceLocateElementChildData):
                    return new BcdDeviceLocateElementChildData(type, addOptions, (LocateDeviceType)(uint)deviceData["Type"], (int)(uint)deviceData["Element"], getDeviceData((ManagementBaseObject)deviceData["Parent"]));

                case nameof(BcdDeviceQualifiedPartitionData):
                    return new BcdDeviceQualifiedPartitionData(type, addOptions, (PartitionStyle)(uint)deviceData["PartitionStyle"], (string)deviceData["DiskSignature"], (string)deviceData["PartitionIdentifier"]);

                case nameof(BcdDeviceUnknownData):
                    return new BcdDeviceUnknownData(type, addOptions, (byte[])deviceData["Data"]);

                case nameof(BcdDeviceData) or nameof(BcdDeviceLocateData):
                    break;

                default:
                    throw new BcdException("Unknown " + property.Origin);
            }
        }

        throw new BcdException("Unknown2");
    }

    private void setElement(BcdElementType type, object value, string paramName, [CallerMemberName] string? methodName = default) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, pathStartString + Id.ToString("B") + pathMiddleString + Store.FilePath + pathEndString, null);
            var inParam = classInstance.GetMethodParameters(methodName);
            inParam["Type"] = type;
            inParam[paramName] = value;

            var outParam = classInstance.InvokeMethod(methodName, inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }
}

using System.Management;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Bluehill.Bcd.Internal;

namespace Bluehill.Bcd;

public sealed record class BcdObject {
    private const string PathStartString = "BcdObject.Id='";
    private const string PathMiddleString = "',StoreFilePath='";
    private const string PathEndString = "'";

    internal BcdObject(BcdStore store, Guid id, BcdObjectType type) {
        Store = store;
        Id = id;
        Type = type;
    }

    public BcdStore Store { get; }
    public Guid Id { get; }
    public BcdObjectType Type { get; }

    public object GetElement(BcdElementType type) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(GetElement));
            inParam["Type"] = type;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(GetElement), inParam, null);
            ReturnValueCheck(outParam);

            foreach (PropertyData property in ((ManagementBaseObject)outParam["Element"]).Properties) {
                switch (property.Name) {
                    case "String" or "Integer" or "Boolean":
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

    public void SetIntegerElement(BcdElementType type, long integer) => setElement(type, (ulong)integer, "Integer");
    public void SetBooleanElement(BcdElementType type, bool boolean) => setElement(type, boolean, "Boolean");
    public void SetStringElement(BcdElementType type, string @string) => setElement(type, @string, "String");

    public void SetPartitionDeviceElement(BcdElementType type, DeviceType deviceType, BcdObject? additionalOptions, string path) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(SetPartitionDeviceElement));
            inParam["Type"] = type;
            inParam["DeviceType"] = deviceType;
            inParam["AdditionalOptions"] = additionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["Path"] = path;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(SetPartitionDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public void SetFileDeviceElement(BcdElementType type, DeviceType deviceType, BcdObject? additionalOptions, string path, DeviceType parentDeviceType, BcdObject? parentAdditionalOptions, string parentPath) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(SetFileDeviceElement));
            inParam["Type"] = type;
            inParam["DeviceType"] = deviceType;
            inParam["AdditionalOptions"] = additionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["Path"] = path;
            inParam["ParentDeviceType"] = parentDeviceType;
            inParam["ParentAdditionalOptions"] = parentAdditionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["ParentPath"] = parentPath;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(SetFileDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public void SetVhdDeviceElement(BcdElementType type, string path, DeviceType parentDeviceType, BcdObject? parentAdditionalOptions, string parentPath, long customLocate) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(SetVhdDeviceElement));
            inParam["Type"] = type;
            inParam["Path"] = path;
            inParam["ParentDeviceType"] = parentDeviceType;
            inParam["ParentAdditionalOptions"] = parentAdditionalOptions?.Id.ToString("B") ?? string.Empty;
            inParam["ParentPath"] = parentPath;
            inParam["CustomLocate"] = customLocate;

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(SetVhdDeviceElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public void SetObjectElement(BcdElementType type, BcdObject @object) {
        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(SetObjectElement));
            inParam["Type"] = type;
            inParam["Id"] = @object.Id.ToString("B");

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(SetObjectElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    public void SetObjectListElement(BcdElementType type, params BcdObject[] objects) {
        if (objects is null || objects.Length == 0) {
            throw new ArgumentException($"'{nameof(objects)}' cannot be Null or empty.", nameof(objects));
        }

        AdminCheck();

        try {
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(nameof(SetObjectListElement));
            inParam["Type"] = type;
            inParam["Ids"] = objects.Select(o => o.Id.ToString("B")).ToArray();

            ManagementBaseObject outParam = classInstance.InvokeMethod(nameof(SetObjectListElement), inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }

    private BcdDeviceData getDeviceData(ManagementBaseObject deviceData) {
        DeviceType type = (DeviceType)(uint)deviceData["DeviceType"];
        var addOptionsString = (string)deviceData["AdditionalOptions"];
        BcdObject? addOptions = string.IsNullOrEmpty(addOptionsString) ? null : Store.OpenObject(Guid.Parse(addOptionsString));

        foreach (PropertyData property in deviceData.Properties) {
            switch (property.Origin) {
                case nameof(BcdDevicePartitionData):
                    return new BcdDevicePartitionData(type, addOptions, (string)deviceData["Path"]);

                case nameof(BcdDeviceFileData):
                    return new BcdDeviceFileData(type, addOptions, getDeviceData((ManagementBaseObject)deviceData["Parent"]), (string)deviceData["Path"]);

                case nameof(BcdDeviceLocateStringData):
                    return new BcdDeviceLocateStringData(type, addOptions, (int)(uint)deviceData["Type"], (string)deviceData["Path"]);

                case nameof(BcdDeviceLocateElementChildData):
                    return new BcdDeviceLocateElementChildData(type, addOptions, (int)(uint)deviceData["Type"], (int)(uint)deviceData["Element"], getDeviceData((ManagementBaseObject)deviceData["Parent"]));

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
            ManagementObject classInstance = new(ScopeString, PathStartString + Id.ToString("B") + PathMiddleString + Store.FilePath + PathEndString, null);
            ManagementBaseObject inParam = classInstance.GetMethodParameters(methodName);
            inParam["Type"] = type;
            inParam[paramName] = value;

            ManagementBaseObject outParam = classInstance.InvokeMethod(methodName, inParam, null);
            ReturnValueCheck(outParam);
        } catch (ManagementException err) {
            throw new BcdException(err);
        }
    }
}

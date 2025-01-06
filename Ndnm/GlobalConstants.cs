namespace Ndnm;

public static class GlobalConstants {
    public const string DnvmName = "dnvm";
    public const string InstancesName = "instances";

    public static readonly string DnvmPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DnvmName);

    public static readonly string InstancesPath = Path.Combine(DnvmPath, InstancesName);
}

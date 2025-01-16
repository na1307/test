namespace Ndnm;

public static class GlobalConstants {
    public const string NdnmName = "ndnm";
    public const string InstancesName = "instances";

    public static readonly string NdnmPath =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), NdnmName);

    public static readonly string InstancesPath = Path.Combine(NdnmPath, InstancesName);
}

using Dotup.Json;
using System.Runtime.InteropServices;

namespace Dotup;

internal static class Constants {
    public const string DirName = "dotup";
    public const string InstancesDirName = "dotnetroot";
    public const string RegistryJsonName = "registry.json";
    public const string CacheDirName = "cache";
    public const string DotnetCliName = "dotnet";
    public const string SdkDirName = "sdk";
    public const string SdkManifestsDirName = "sdk-manifests";
    public const string HostDirName = "host";
    public const string FxrDirName = "fxr";
    public const string PacksDirName = "packs";
    public const string NetCoreAppHostDirName = "Microsoft.NETCore.App.Host";
    public const string NetCoreAppRefDirName = "Microsoft.NETCore.App.Ref";
    public const string AspNetCoreAppRefDirName = "Microsoft.AspNetCore.App.Ref";
    public const string SharedDirName = "shared";
    public const string NetCoreAppDirName = "Microsoft.NETCore.App";
    public const string AspNetCoreAppDirName = "Microsoft.AspNetCore.App";
    public const string AspNetCoreAllDirName = "Microsoft.AspNetCore.All";
    public const string TemplatesDirName = "templates";
    public const string EnvName = "DOTUP_ROOT";
    public static readonly Uri ReleasesIndexJson = new("https://builds.dotnet.microsoft.com/dotnet/release-metadata/releases-index.json");
    public static readonly DotupJsonSerializerContext Djsc;

    public static readonly JsonWriterOptions IndentedWriterOptions = new() {
        Indented = true
    };

    [SuppressMessage("Performance", "CA1810:참조 형식 정적 필드 인라인을 초기화하세요.", Justification = "JSON")]
    static Constants() {
        JsonSerializerOptions serializerOptions = new();

        serializerOptions.Converters.Add(new Sha512Converter());
        serializerOptions.Converters.Add(new SemVersionConverter());

        Djsc = new(serializerOptions);
    }

    public static string DotupPath {
        get {
            var envString = Environment.GetEnvironmentVariable(EnvName)!;

            return string.IsNullOrWhiteSpace(envString)
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DirName)
                : envString;
        }
    }

    public static string RegistryJsonPath => Path.Combine(DotupPath, RegistryJsonName);

    public static string InstancesPath => Path.Combine(DotupPath, InstancesDirName);

    public static string CachePath => Path.Combine(DotupPath, CacheDirName);

    public static string DotnetPath => Path.Combine(DotupPath, InstancesDirName, DotnetCliName);

    // ReSharper disable once InconsistentNaming
    public static string OSName {
        get {
            if (OperatingSystem.IsLinux()) {
                return "linux";
            }

            throw new PlatformNotSupportedException("This is not supported on current operating system.");
        }
    }

    public static string Arch
        => RuntimeInformation.OSArchitecture switch {
            Architecture.X64 => "x64",
            Architecture.X86 => "x86",
            Architecture.Arm64 => "arm64",
            Architecture.Arm => "arm",
            _ => throw new PlatformNotSupportedException("This is not supported on current operating system.")
        };

    public static string RuntimeIdentifier => $"{OSName}-{Arch}";
}

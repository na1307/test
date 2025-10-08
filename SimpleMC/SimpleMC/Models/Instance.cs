using CommunityToolkit.Mvvm.DependencyInjection;
using System.Text.Json;

namespace SimpleMC.Models;

internal sealed class Instance {
    private const string InstanceJsonFileName = "instance.json";
    private FileInfo? instanceJsonFile;

    public required Guid InstanceId { get; set; }

    public required string Name { get; set; }

    public required string Version { get; set; }

    public static DirectoryInfo GetInstancesDir() {
        var instancePath = Ioc.Default.GetRequiredService<SettingsService>().Paths.InstancePath;

        return new(!Path.IsPathRooted(instancePath) ? Path.Combine(App.BaseDirectoryPath, instancePath) : instancePath);
    }

    public static IEnumerable<Instance> GetAllInstances() {
        if (!GetInstancesDir().Exists) {
            return [];
        }

        var instanceJsons = GetInstancesDir().EnumerateDirectories().SelectMany(d => d.EnumerateFiles(InstanceJsonFileName));

        return instanceJsons.Select(ReadInstanceJson).OfType<Instance>();
    }

    public static void CreateNewInstance(string name, string version) {
        GetInstancesDir().Create();

        var ig = Guid.NewGuid();
        var instanceDir = Directory.CreateDirectory(Path.Combine(GetInstancesDir().FullName, ig.ToString("D")));

        Instance i = new() {
            InstanceId = ig,
            Name = name,
            Version = version,
            instanceJsonFile = new(Path.Combine(instanceDir.FullName, InstanceJsonFileName))
        };

        i.SaveInstanceJson();
    }

    public string GetInstanceDir() => Path.Combine(GetInstancesDir().FullName, InstanceId.ToString("D"));

    public void SaveInstanceJson() {
        if (instanceJsonFile is null) {
            throw new InvalidOperationException("Invalid object state: instanceJsonFile is null");
        }

        if (!instanceJsonFile.Directory!.Exists) {
            throw new DirectoryNotFoundException("Instance directory does not exist");
        }

        using FileStream fs = new(instanceJsonFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None);

        using Utf8JsonWriter jw = new(fs, new() {
            Indented = true
        });

        JsonSerializer.Serialize(jw, this);
    }

    private static Instance? ReadInstanceJson(FileInfo instanceJson) {
        if (!instanceJson.Exists) {
            return null;
        }

        try {
            var instance = JsonSerializer.Deserialize<Instance>(File.ReadAllBytes(instanceJson.FullName), JsonSerializerOptions.Strict);

            if (instance is null) {
                return null;
            }

            instance.instanceJsonFile = instanceJson;

            return instance;
        } catch (JsonException je) {
            Console.WriteLine($"Invalid instance.json: {je.Message}"); // Will replaced by logger

            return null;
        }
    }
}

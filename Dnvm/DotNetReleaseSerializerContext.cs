using System.Text.Json.Serialization;

namespace Dnvm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(DotNetRelease))]
public sealed partial class DotNetReleaseSerializerContext : JsonSerializerContext;

using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(DotNetRelease))]
public sealed partial class DotNetReleaseSerializerContext : JsonSerializerContext;

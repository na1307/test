using System.Text.Json.Serialization;

namespace Dnvm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(SdkRelease))]
public sealed partial class SdkReleaseSerializerContext : JsonSerializerContext;

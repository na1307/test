using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(SdkRelease))]
public sealed partial class SdkReleaseSerializerContext : JsonSerializerContext;

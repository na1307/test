using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(SdkFile))]
public sealed partial class SdkFileSerializerContext : JsonSerializerContext;

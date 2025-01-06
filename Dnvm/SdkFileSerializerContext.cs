using System.Text.Json.Serialization;

namespace Dnvm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(SdkFile))]
public sealed partial class SdkFileSerializerContext : JsonSerializerContext;

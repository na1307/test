using System.Text.Json.Serialization;

namespace Dnvm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(DotNetChannel))]
public sealed partial class DotNetChannelSerializerContext : JsonSerializerContext;

using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(DotNetChannel))]
public sealed partial class DotNetChannelSerializerContext : JsonSerializerContext;

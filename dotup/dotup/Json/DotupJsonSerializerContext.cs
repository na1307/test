using Dotup.Json.GlobalConfig;
using Dotup.Json.Registry;
using Dotup.Json.ReleasesIndex;

namespace Dotup.Json;

[JsonSerializable(typeof(DotnetReleasesIndex))]
[JsonSerializable(typeof(DotnetChannel))]
[JsonSerializable(typeof(DotupRegistry))]
[JsonSerializable(typeof(GlobalJson))]
internal sealed partial class DotupJsonSerializerContext : JsonSerializerContext;

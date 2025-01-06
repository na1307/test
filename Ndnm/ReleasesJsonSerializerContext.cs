using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSerializable(typeof(DotNetChannel))]
[JsonSerializable(typeof(DotNetRelease))]
[JsonSerializable(typeof(SdkRelease))]
[JsonSerializable(typeof(SdkFile))]
public sealed partial class ReleasesJsonSerializerContext : JsonSerializerContext;

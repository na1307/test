using System.Text.Json.Serialization;

namespace Ndnm;

[JsonSerializable(typeof(DotNetChannel))]
[JsonSerializable(typeof(DotNetRelease))]
[JsonSerializable(typeof(DotNetSdkRelease))]
[JsonSerializable(typeof(DotNetSdkFile))]
public sealed partial class ReleasesJsonSerializerContext : JsonSerializerContext;

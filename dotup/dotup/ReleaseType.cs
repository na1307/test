namespace Dotup;

[JsonConverter(typeof(JsonStringEnumConverter<ReleaseType>))]
internal enum ReleaseType {
    Lts,
    Sts
}

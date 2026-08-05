namespace Dotup;

internal sealed class SemVersionConverter : JsonConverter<SemVersion> {
    public override SemVersion? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();

        return str is not null ? SemVersion.Parse(str) : null;
    }

    public override void Write(Utf8JsonWriter writer, SemVersion value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}

namespace Dotup;

internal sealed class Sha512Converter : JsonConverter<byte[]> {
    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString();

        return value is not null ? Convert.FromHexString(value) : null;
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options) => throw new NotImplementedException();
}
